import * as cdk from '@aws-cdk/core';
import * as rds from '@aws-cdk/aws-rds';
import * as ec2 from '@aws-cdk/aws-ec2';
import * as iam from '@aws-cdk/aws-iam';
import * as s3 from '@aws-cdk/aws-s3';
import { DatabaseInstance, ClusterParameterGroup, ParameterGroup } from '@aws-cdk/aws-rds';
import { ConstructNode, RemovalPolicy } from '@aws-cdk/core';


export class RdsStack extends cdk.Stack {
  constructor(scope: cdk.Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    // setup s3 bucket for bulk s3 imports
    // TODO: create condition to create s3 if it doesn't exist
    const bucketName = 'warp-initialload-dev';
    const s3Bucket = new s3.Bucket(this, 'RDS_S3_Bucket', {
      removalPolicy: RemovalPolicy.DESTROY,
      bucketName: bucketName,
      blockPublicAccess: s3.BlockPublicAccess.BLOCK_ALL
    });

    // setup iam role
    const iamRole = new iam.Role(this, 'RDS_IAM_ROLE', {
      assumedBy: new iam.ServicePrincipal("rds.amazonaws.com"),
      roleName: 'rds-s3-import-role'
    });

    // setup iam policy for RDS access to S3 and add role
    const policyStatement = new iam.PolicyStatement({
      effect: iam.Effect.ALLOW,
      resources: [s3Bucket.bucketArn]
    });

    policyStatement.addActions("s3:GetObject", "s3:ListBucket");

    const iamPolicy = new iam.Policy(this, 'RDS_IAM_POLICY', {
      policyName: 'rds-s3-import-policy',
      statements: [policyStatement],
      roles: [iamRole],
    });

    // get existing vpc
    // TODO: pass in vpcName from parameters
    const vpc = ec2.Vpc.fromLookup(this, 'VPC', {
      isDefault: false, 
      vpcName: '1-243990056803'
    });

    const rdsSecurityGroupVPN = new ec2.SecurityGroup(this, 'NewSecurityGroup', {
      description: 'Allow VPN Access to RDS',
      securityGroupName: 'rds-vpn-access',
      allowAllOutbound: true,
      vpc: vpc
    });
    rdsSecurityGroupVPN.addIngressRule(ec2.Peer.ipv4('10.0.0.0/8'), ec2.Port.tcp(3306));

    const paramGroup = ClusterParameterGroup.fromParameterGroupName(this, 'RdsClusterParameterGroup', 'default.aurora-postgresql11');

    // create RDS Db Cluster
    const rdsInstance = new rds.DatabaseCluster(this, 'AuroraRdsInstance', {
      parameterGroup: paramGroup,
      defaultDatabaseName: 'warp',
      masterUser: {
        username: 'warpadmindev'
      },
      engine: rds.DatabaseClusterEngine.AURORA_POSTGRESQL,
      instanceProps: {
        instanceType: ec2.InstanceType.of(ec2.InstanceClass.MEMORY5, ec2.InstanceSize.LARGE),
        vpcSubnets: {
          subnetType: ec2.SubnetType.PRIVATE
        },
        vpc: vpc,
        securityGroups: [rdsSecurityGroupVPN]
      },
      engineVersion: '11.7',
      s3ImportRole: iamRole
    });
  }
}
