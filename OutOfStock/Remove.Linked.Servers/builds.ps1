

properties {
	$config = 'debug'; #debug or release
}

task -name ValidateConfig -action {
	assert ('debug', 'release' -contains $config) `
		"Invalid config: $config; valid values are debug and release";
}


task -name Clean  -depends ValidateConfig -description "deletes all build artifacts" -action {
	exec {
		msbuild .\OutOfStock.sln /t:Clean
	}
}


task -name Build -depends ValidateConfig -description "builds outdated artifacts" -action {
	exec {
		msbuild .\OutOfStock.sln /t:Build
	}

}
task -name Rebuild -depends Clean,Build -description "rebuilds all artifacts from source"

task -name Default -depends Build;

