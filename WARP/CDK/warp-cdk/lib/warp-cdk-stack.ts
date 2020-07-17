import * as cdk from '@aws-cdk/core';
import * as lambda from '@aws-cdk/aws-lambda';
import * as iam from '@aws-cdk/aws-iam';
import * as sqs from '@aws-cdk/aws-sqs';
import * as rds from '@aws-cdk/aws-rds';
import * as eventSources from '@aws-cdk/aws-lambda-event-sources';
import * as ec2 from '@aws-cdk/aws-ec2';
import * as s3 from '@aws-cdk/aws-s3';
import * as ecr from '@aws-cdk/aws-ecr';
import * as ecs from '@aws-cdk/aws-ecs';
import * as path from 'path';


/*

IMPORTANT NOTE: this cdk script requires a change to be made to node_modules\@aws-cdk\aws-rds\lib\cluster.js
Or you recieve the following error:
  The feature-name parameter must be provided with the current operation for the Aurora (PostgreSQL) engine.

In @aws-cdk/aws-rds": "1.51.0", starting @ line 111. featureName: 's3Import' must be added.

              if (s3ImportRole) {
                clusterAssociatedRoles.push({ roleArn: s3ImportRole.roleArn, featureName: 's3Import' });
            }

Something similar for s3Export is likely required but we will cross that bridge when we get there.
Here is the related bug: https://github.com/aws/aws-cdk/issues/8201
Scheduled to be fixed in 9/2020 milestone

*/


export class WarpCdkStack extends cdk.Stack {

  constructor(scope: cdk.App, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    // ### get the existing VPC, make sure its the correct one
    // ###################################################################
    const vpc = ec2.Vpc.fromLookup(this, 'WarpVpc', {
      isDefault: false,
      vpcName: '1-243990056803'
    });

    // ### setup s3 bucket for bulk s3 imports
    // ###################################################################

    // TODO: create condition to create s3 if it doesn't exist
    const bucketName = 'warp-initialload-dev1';
    const s3Bucket = new s3.Bucket(this, 'WarpS3', {
      removalPolicy: cdk.RemovalPolicy.DESTROY,
      bucketName: bucketName,
      blockPublicAccess: s3.BlockPublicAccess.BLOCK_ALL
    });

    // ### setup iam role for s3 bucket.
    // ###################################################################
    const iamRole = new iam.Role(this, 'WarpRdsIamRole', {
      assumedBy: new iam.ServicePrincipal("rds.amazonaws.com"),
      roleName: 'rds-s3-import-role1'
    });

    // setup iam policy for RDS access to S3 and add role
    // ###################################################################
    const policyStatement = new iam.PolicyStatement({
      effect: iam.Effect.ALLOW,
      resources: [s3Bucket.bucketArn]
    });

    policyStatement.addActions("s3:GetObject", "s3:ListBucket");

    const iamPolicy = new iam.Policy(this, 'WarpS3ImportPolicy', {
      policyName: 'rds-s3-import-policy1',
      statements: [policyStatement],
      roles: [iamRole],
    });

    // ### create security group for secretmanager enpoint
    // ###################################################################
    var secretsManagerSecurityGroup = new ec2.SecurityGroup(this, 'WarpSecretsManagerSecurityGroup', {
      vpc: vpc,
      allowAllOutbound: true,
      securityGroupName: 'WarpSecretsManagerSecurityGroup',
      description: 'Allow Lambda Access to Secretsmanager',
    });

    // ### create a VPC Enpoint for Secrets Manager
    // ###################################################################

    var secretManagerEndpoint = new ec2.InterfaceVpcEndpoint(this, 'WarpSecretsManagerEndpoint', {
      vpc,
      service: new ec2.InterfaceVpcEndpointService('com.amazonaws.us-east-1.secretsmanager'),
      // Choose which availability zones to place the VPC endpoint in, based on
      // available AZs
      subnets: {
        availabilityZones: vpc.availabilityZones
      },
      privateDnsEnabled: true,
      securityGroups: [secretsManagerSecurityGroup]
    });

    // iam user needs permissions for these.... ecr:DescribeRepositories
    // ECR repository
    // ###################################################################

    const repository = new ecr.Repository(this, "warp-app-ecr", {
      repositoryName: "warp-app-ecr",
      removalPolicy: cdk.RemovalPolicy.DESTROY
    });

    // ECS cluster/resources
    // ###################################################################

    const cluster = new ecs.Cluster(this, "warp-app-ecs", {
      clusterName: "warp-app-cluster",
      vpc: vpc,
    });

    // create a role that the lambda will execute as
    // ###################################################################

    const lambdaExecutionRole = new iam.Role(this, 'WarpLambdaExecutionRole', {
      roleName: 'WarpLambdaExecutionRole',
      assumedBy: new iam.ServicePrincipal('lambda.amazonaws.com'),
      managedPolicies: [
        iam.ManagedPolicy.fromAwsManagedPolicyName("service-role/AWSLambdaVPCAccessExecutionRole"),
        iam.ManagedPolicy.fromAwsManagedPolicyName("service-role/AWSLambdaBasicExecutionRole"),
        iam.ManagedPolicy.fromManagedPolicyArn(this, 'WarpSecretsManagerRWPolicy', "arn:aws:iam::aws:policy/SecretsManagerReadWrite")
      ]
    });

    const lambdaSecurityGroup = new ec2.SecurityGroup(this, 'WarpLambdaSecurityGroup', {
      vpc: vpc,
      allowAllOutbound: true,
      securityGroupName: 'WarpLambdaSecurityGroup',
      description: 'Allow Lambda Access to RDS'
    });

    // ### database 
    // ###################################################################

    const rdsSecurityGroupVPN = new ec2.SecurityGroup(this, 'WarpRDSSecurityGroup', {
      description: 'Allow VPN Access to RDS',
      securityGroupName: 'WarpRDSSecurityGroup',
      allowAllOutbound: true,
      vpc: vpc
    });

    rdsSecurityGroupVPN.addIngressRule(ec2.Peer.ipv4('10.0.0.0/8'), ec2.Port.tcp(3306));

    const paramGroup = rds.ClusterParameterGroup.fromParameterGroupName(this, 'WarpRDSClusterParameterGroup', 'default.aurora-postgresql11');

    var db = new rds.DatabaseCluster(this, 'WarpRDSInstance', {
      parameterGroup: paramGroup,
      defaultDatabaseName: 'warp',
      masterUser: {
        username: 'warpadmindev',
      },
      clusterIdentifier: 'WarpCluster',
      instanceIdentifierBase: 'WarpInsatnce',
      engine: rds.DatabaseClusterEngine.AURORA_POSTGRESQL,
      port: 3306,
      engineVersion: '11.7',
      s3ImportRole: iamRole,
      instanceProps: {
        instanceType: ec2.InstanceType.of(ec2.InstanceClass.MEMORY5, ec2.InstanceSize.LARGE),
        vpcSubnets: {
          subnetType: ec2.SubnetType.PRIVATE
        },
        vpc: vpc,
        securityGroups: [rdsSecurityGroupVPN, lambdaSecurityGroup],
      }
    });

    // ### create an SQS Queue that will be an event source for the lambda.
    // ###################################################################
    const sqsQueue = new sqs.Queue(this, 'WarpPriceQueue', { queueName: 'WarpPriceQueue' });

    // ### create the lambda function.
    // ###################################################################
    const LambdaFunction = new lambda.Function(this, 'WarpLambdaProcessPrices', {
      runtime: lambda.Runtime.DOTNET_CORE_3_1,
      handler: 'Warp.ProcessPrices::Warp.ProcessPrices.Lambda.Function::FunctionHandler',
      code: lambda.Code.fromAsset(path.join(__dirname, '..\\lambda\\Warp.ProcessPrices.Lambda.zip')),
      role: lambdaExecutionRole,
      securityGroup: lambdaSecurityGroup,
      vpc: vpc,
      timeout: cdk.Duration.seconds(30),
      memorySize: 512,
      vpcSubnets: { subnetType: ec2.SubnetType.PRIVATE },
      // Lambda Expects DatabaseSecretName environment variable that points to SecretManager credentials for database user. must has username/password/host/port
      environment: { 'DatabaseSecretName': db.secret!.secretArn }
    });

    // map sqsQueue as event source for lambda.
    // ###################################################################
    const eventSource = LambdaFunction.addEventSource(new eventSources.SqsEventSource(sqsQueue));
  }
}
