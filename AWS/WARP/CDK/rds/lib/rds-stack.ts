import * as cdk from '@aws-cdk/core';
import * as rds from '@aws-cdk/aws-rds';
import * as ec2 from '@aws-cdk/aws-ec2';
import { DatabaseInstance } from '@aws-cdk/aws-rds';

export class RdsStack extends cdk.Stack {
  constructor(scope: cdk.Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    const vpc = ec2.Vpc.fromLookup(this, 'VPC', {
      isDefault: true
    });

    new rds.DatabaseCluster(this, 'AuroraRdsInstance', {
      defaultDatabaseName: 'warp-dev',
      masterUser: {
        username: 'warp-admin-dev'
      },
      engine: rds.DatabaseClusterEngine.AURORA_POSTGRESQL,
      instanceProps: {
        instanceType: ec2.InstanceType.of(ec2.InstanceClass.MEMORY5_HIGH_PERFORMANCE, ec2.InstanceSize.MEDIUM),
        vpcSubnets: {
          subnetType: ec2.SubnetType.PRIVATE
        },
        vpc: vpc
      }
    });
  }
}
