// javascript for Esb envrionments/app configuration views (Esb/Index)
//  (requires vue.js, sortable.js, vuedraggable.js)

// load variables which should have been globally set in the razor view
var postUrl         = (urlForPost) ? urlForPost : "/Esb/Index";
var detailsUrl      = (urlForPost) ? urlForEnvDetails : "/Esb/Details";
var appConfigUrl    = (urlForPost) ? urlForAppConfig : "/Home/Edit";
var idForVueElement = (vueElementSelector) ? vueElementSelector : "#vueElement";
var idForSaveButton = (btnSaveSelector) ? btnSaveSelector :"#saveChangesBtn";
var idForLoadingImg = (btnLoadingImgSelector) ? btnLoadingImgSelector : "#saveChangesLoadingImg";
var vueModel        = {};

// function for posting to server with ajax
function xhr_post_json(url, data) {
    if (console) console.log("xhr_post_json() function called");

    $.ajax({
        beforeSend: function (e) {
            //disable save button & show loading icon
            $(idForSaveButton).attr("disabled", "disabled");
            $(idForLoadingImg).removeClass("hidden");
        },
        type: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(data),
    }).always(function (data_or_jqXHR, textStatus, jqXHR_or_errorThrown) {
        // always() gets called back whether post succeeded or failed
        // re-enable the save button & hide the loading icon
        $(idForSaveButton).removeAttr("disabled");
        $(idForLoadingImg).addClass("hidden");
    }).fail(function (jqXHR, textStatus, errorThrown) {
        // .fail() gets called back after an error with the message or decoding the response
        var info = textStatus + ": [" + jqXHR.status + ": '" + jqXHR.statusText + "'], errorThrown: '" + errorThrown + "'";
        if (console) console.log("$.ajax(POST:'" + url + "').fail(" + info + ")");
    }).done(function (data, textStatus, jqXHR) {
        // .done() gets called back after successful completion
        var info = textStatus + " [" + jqXHR.status + ": '" + jqXHR.statusText + "']";
        if (console) console.log("$.ajax(POST:'" + url + "').done(" + info + ")");
    });
}

// object to hold functions that can be accessed via the vue object
var vueMethods = {
    submitVueModelToServer: function (e) {
        // this can be called from the vue object ("this"),
        //  which should have the js data object(s) to post
        xhr_post_json(postUrl, this.$data);
    },
    //checkMove: function (evt) {
    //    var x = evt.draggedContext.element.name;
    //    return true;
    //},
    //addApp: function (param, event) {
    //    console.log("addThing?");

    //    var envName = $(event.currentTarget).attr("data-env-name");
    //    var envIndex = $(event.currentTarget).attr("data-env-index");

    //    //this.$set(this.testArray, 1, 48);
    //    this.esbEnvironments[envIndex].AppsInEnvironment.push({
    //        id: this.esbEnvironments[envIndex].AppsInEnvironment.length,
    //        parentId: envIndex,
    //        Name: this.newAppName,
    //        Server: this.newServerName,
    //        DisplayName: this.newAppName,
    //        ConfigFilePath: "xxxx",
    //        AppConfigUrl: "adsfsa"
    //    });
    //},
    //removeApp: function (param, event) {
    //    console.log("removeThing?");

    //    var envName = $(event.currentTarget).attr("data-env-name");
    //    var envIndex = $(event.currentTarget).attr("data-env-index");
    //    var appServer = $(event.currentTarget).attr("data-app-server");
    //    var appName = $(event.currentTarget).attr("data-app-name");
    //    var appIndex = $(event.currentTarget).attr("data-app-index");

    //    this.$data.esbEnvironments[envIndex].AppsInEnvironment.splice(appIndex, 1);
    //},   
    //dragStart: function () {
    //    console.log("dragStart...");
    //    drag = true;
    //},
    //dragEnd: function () {
    //    console.log("dragEnd...");
    //    drag = false;
    //},
    //onDragOver: function (e) {
    //    if (console) console.log("onDragOver()")
    //},
    //onDragEnd: function (e) {
    //    if (console) console.log("onDragEnd()")
    //}  
};

// object to hold data for vue object should have been declared from razor view

$(document).ready(function () {
    // create javascript Vue object for binding lists to controls (requires vue.js, sortable.js, vuedraggable.js)
    // we will use existing objects for the data & methods properties in the Vue.js object
    //  these data/methods objects should have been created above or from the razor-generated view
    vueModel = new Vue({
        el: idForVueElement,
        data: vueData,
        //watch: {
        //    testVariable: function (val) {
        //        console.log("watch...");
        //    }
        //},
        methods: vueMethods,
        //beforeCreate: function () {
        //    console.log("Vue.js beforeCreate event")
        //},
        //created: function () {
        //    console.log ("Vue.js created event:  " + this.esbEnvironments.length + " environment elements")
        //},
        //beforeMount: function () {
        //    console.log("Vue.js beforeMount event")
        //},
        //mounted: function () {
        //    console.log("Vue.js mounted event")
        //},
        //beforeUpdate: function () {
        //    if (console) console.log("Vue.js beforeUpdate event")
        //},
        //updated: function () {
        //    if (console) console.log("Vue.js updated event")
        //},
        //onDragStart: function (e) {
        //    if (console) console.log("onDragStart()")
        //    //e.dataTransfer.setData("text/html", e.currentTarget);
        //},       
    });
});