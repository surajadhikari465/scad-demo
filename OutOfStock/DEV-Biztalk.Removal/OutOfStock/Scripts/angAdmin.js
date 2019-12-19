var adminApp = angular.module("adminApp", ["ngAnimate", "ajoslin.promise-tracker"]);

adminApp.config(["$httpProvider", function ($httpProvider) {
    $httpProvider.interceptors.push("noCacheInterceptor");
}]).factory("noCacheInterceptor", function () {
    return {
        request: function (config) {
            if (config.method === "GET") {
                var separator = config.url.indexOf("?") === -1 ? "?" : "&";
                config.url = config.url + separator + "noCache=" + new Date().getTime();
            }
            return config;
        }
    };
});

adminApp.controller("updaterCtrl", function($scope, $http, promiseTracker) {
    $scope.loadingTracker = promiseTracker();
    $scope.appData = null;
    $scope.regionList = null;
    $scope.selectedRegion = null;
    $scope.newStoreRegion = null;
    $scope.initialized = ($scope.selectedRegion != null);
    $scope.updatesAvailable = false;
    $scope.view = null;

    

    $scope.getRegionList = function() {
        $http.get("/Admin/GetRegionList", { tracker: $scope.loadingTracker }).success(function (data) {
            $scope.regionList = data;
        });
    }


    $scope.loadStoresCurrentlyInOOS = function () {
        $http.get("/Admin/GetAvailableStores?regionid=" + $scope.selectedRegion.Value, { tracker: $scope.loadingTracker }).success(function(data) {
            $scope.appData = data;
            $scope.view = "oos";
            $scope.checkForUpdates();
        });
    };

    $scope.closeStore = function() {

        var data = {
            RegionId: $scope.selectedRegion.Value,
            StoreId: "",
            PSBU: ""
        };
        data.PSBU = $("#closestoreid").val();

        if (data.PSBU === "")
            alert("PSBU is required.");
        else {
            $http({ method: "POST", url: "/Admin/CloseStore", data: data }).success(function (d, status, headers, config) {
                var m = $("#CloseStoreMessage");
                m.text(d.message);

                if (d.isError) {
                    m.css("color", "red");
                } else {
                    m.css("color", "green");
                    $("#closestoreid").val("");
                }

                m.show();
            });
        }
    }

    $scope.renameStore= function() {
        var data = {
            StoreId: "",
            StoreName: "",
            StoreAbbr: "",
            StoreNumber: ""
        };
        data.StoreNumber = $("#renamestoreid").val();
        data.StoreName = $("#renamestorename").val();
        data.StoreAbbr = $("#renamestoreabbr").val();

        if (data.StoreNumber === "")
            alert("PSBU is required.");
        else {
            $http({ method: "POST", url: "/Admin/RenameStore", data: data }).success(function (d, status, headers, config) {
                
                var m = $("#RenameStoreMessage");
                m.text(d.message);

                if (d.isError) {
                    m.css("color", "red");
                } else {
                    m.css("color", "green");
                    $("#renamestoreid").val("");
                    $("#renamestorename").val("");
                    $("#renamestoreabbr").val("");
                }

                m.show();
            });
        }
    }

    $scope.displayHideStore = function() {
        var data = {
            StoreAbbr: "",
            StoreName: "",
            StoreNumber: "",
            RegionId: $scope.selectedRegion.Value,
            View: ""
        }

        data.StoreNumber = $("#displayStorePSBU").val();

        if (data.StoreNumber === "") {


            // error
            alert("Business Unit is required.");
       
        } else {
            $http({ method: "POST", url: "/Admin/ToggleStoreVisibility", data: data }).success(function (d, status, headers, config) {
                var m = $("#VisibilityMessage");
                m.text(d.message);

                if (d.isError) {
                    m.css("color", "red");
                } else {
                    m.css("color", "green");
                }
                $("#displayStorePSBU").val("");
                m.show();
            });
        }


    }

    $scope.addNewStore = function() {
        var data = {
            StoreAbbr: "",
            StoreName: "",
            StoreNumber: "",
            RegionId: $scope.selectedRegion.Value,
            View: ""
        }

        data.StoreAbbr = $("#newstoreabbreviation").val();
        data.StoreName = $("#newstorename").val();
        data.StoreNumber = $("#newstorebu").val();

        if (data.StoreAbbr === "" || data.StoreName === "" || data.StoreNumber === "") {


            // error
            alert("Business Unit, Name, and Abbreviation are required.");
            console.log(data.StoreAbbr);
            console.log(data.StoreName);
            console.log(data.StoreNumber);
            console.log(data.RegionId);

        } else {
            console.log("add");
            $http({ method: "POST", url: "/Admin/AddStore", data: data }).success(function(d, status, headers, config) {
                console.log(d);
                var m = $("#AddStoreMessage");
                m.text(d.message);

                if (d.isError) {
                    m.css("color", "red");
                } else {
                    m.css("color", "green");
                     $("#newstoreabbreviation").val("");
                    $("#newstorename").val("");
                    $("#newstorebu").val("");
                }

                m.show();
            });
        }

    }


    $scope.updateThisApp = function(view) {
      
        if (view === "oos") return;

        $.each($(".storeSelect:checked"), function (i, v) {

            var data = $($(v)[0].previousSibling)[0].value;
            var json = JSON.parse(data);
            json["View"] = view;
            


            $http({ method: "POST", url: "/Admin/SaveStore", data: json }).success(function(d, status, headers, config) {
                
                $scope.checkForUpdates();
                switch (view) {
                    case "updates":
                        $scope.loadStoresThatNeedUpates();
                        break;
                    case "new":
                        $scope.loadStoresToBeAdded();
                        break;
                    case "oos":
                        $scope.loadStoresCurrentlyInOOS();
                        break;
                }
            });

            

            
        });
    }

    $scope.loadStoresToBeAdded = function () {        
        $http.get("/Admin/GetNewStores", { tracker: $scope.loadingTracker }).success(function(data) {
            $scope.appData = data;
            $scope.view = "new";
        });
    };

    $scope.loadStoresThatNeedUpates = function() {
        $http.get("/Admin/GetUpdatedStores", { tracker: $scope.loadingTracker }).success(function(data) {
            $scope.appData = data;
            $scope.view = "updates";
        });
    };


    $scope.getRecentScansByRegion = function() {
        $("#checkLoader").show();

        var url = "/Admin/GetRecentScansByRegion?regionid=" + $scope.selectedRegion.Value + "&regionabbr=" + $scope.selectedRegion.Text;
        $http.get(url, { tracker: $scope.loadingTracker })
            .success(function(data) {
            console.log(data);
        });

        $("#checkLoader").hide();
    };

    $scope.checkForUpdates = function() {
        $("#checkLoader").show();
        $scope.initialized = true;
        $scope.updatesAvailable = false;
        //$scope.appData = null;

        var url = "/Admin/CompareStoresByRegion?regionid=" + $scope.selectedRegion.Value + "&regionabbr=" + $scope.selectedRegion.Text;

        $http.get(url, { tracker: $scope.loadingTracker })
            .success(function(data) {


                for (var i = 0; i < data.length; i++) {
                    //console.log(data[i].Key + " :: " + data[i].Value);

                    if (data[i].Key === "update") {
                        $scope.updateStoreCount = data[i].Value;
                        if (data[i].Value > 0) {
                            $scope.updatesAvailable = true;
                        }
                    } else if (data[i].Key === "insert") {
                        $scope.newStoreCount = data[i].Value;
                        if (data[i].Value > 0) {
                            $scope.updatesAvailable = true;
                        }
                    } else {
                        $scope.currentStoreCount = data[i].Value;
                     
                    }
                }
            });
    };

    $scope.getRegionList();
});

adminApp.controller("userManagementCtrl",
    function($scope, $http, promiseTracker) {
        $scope.loadingTracker = promiseTracker();
        $scope.matchedUsers = null;

        $scope.getUserInfo = function() {

            var user = $("#txtUserSearch").val();

            $http.get("/Admin/GetUserInfo?user="+user, { tracker: $scope.loadingTracker }).success(function (data) {
                $scope.matchedUsers = data;
            });

        }

        $scope.saveOverride = function(user, uniqueId) {
            
            var regionOverride = $("#ro" + uniqueId).val();
            var storeOverride = $("#so" + uniqueId).val();

            var data = {
                Username: user,
                Region: regionOverride,
                Store: storeOverride
            };
            console.log(data);

            $http({ method: "POST", url: "/Admin/SaveLocationOverride", data: data }).success(function (d, status, headers, config) {
                console.log(d);
            });


        }

    });