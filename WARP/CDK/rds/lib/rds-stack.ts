import * as cdk from '@aws-cdk/core';
import * as rds from '@aws-cdk/aws-rds';
import * as ec2 from '@aws-cdk/aws-ec2';
import { DatabaseInstance, ClusterParameterGroup, ParameterGroup } from '@aws-cdk/aws-rds';
import { ConstructNode } from '@aws-cdk/core';


export class RdsStack extends cdk.Stack {
  constructor(scope: cdk.Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

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

    new rds.DatabaseCluster(this, 'AuroraRdsInstance', {
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
      }
    });
  }
}
