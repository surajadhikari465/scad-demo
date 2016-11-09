var animalWelfareRatings = {};
var milkTypes = {};
var ecoScaleRatings = {};
var productionClaims = {};
var seafoodFreshOrFrozenTypes = {};
var seafoodCatchTypes = {};
var glutenFreeAgencies = {};
var kosherAgencies = {};
var nonGmoAgencies = {};
var organicAgencies = {};
var veganAgencies = {};

function loadAnimalWelfareRatings() {
    $.getJSON("CertificationAgency/GetAnimalWelfareRatings", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            animalWelfareRatings[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadMilkTypes() {
    $.getJSON("CertificationAgency/GetMilkTypes", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            milkTypes[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadEcoScaleRatings() {
    $.getJSON("CertificationAgency/GetEcoScaleRatings", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            ecoScaleRatings[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadSeafoodFreshOrFrozenTypes() {
    $.getJSON("CertificationAgency/GetSeafoodFreshOrFrozenTypes", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            seafoodFreshOrFrozenTypes[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadSeafoodCatchTypes() {
    $.getJSON("CertificationAgency/GetSeafoodCatchTypes", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            seafoodCatchTypes[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadGlutenFreeAgencies() {
    $.getJSON("CertificationAgency/GetGlutenFreeAgencies", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            glutenFreeAgencies[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadKosherAgencies() {
    $.getJSON("CertificationAgency/GetKosherAgencies", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            kosherAgencies[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadNonGmoAgencies() {
    $.getJSON("CertificationAgency/GetNonGmoAgencies", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            nonGmoAgencies[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadOrganicAgencies() {
    $.getJSON("CertificationAgency/GetOrganicAgencies", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            organicAgencies[agencies[i].id] = agencies[i].name;
        }
    });
}
function loadVeganAgencies() {
    $.getJSON("CertificationAgency/GetVeganAgencies", function (agencies) {
        for (var i = 0; i < agencies.length; i++) {
            veganAgencies[agencies[i].id] = agencies[i].name;
        }
    });
}

function formatAnimalWelfareRating(val) {
    if (animalWelfareRatings[val]) {
        return animalWelfareRatings[val];
    } else {
        return '';
    }
}

function formatMilkType(val) {
    if (milkTypes[val]) {
        return milkTypes[val];
    } else {
        return '';
    }
}

function formatEcoScaleRating(val) {
    if (ecoScaleRatings[val]) {
        return ecoScaleRatings[val];
    } else {
        return '';
    }
}

function formatProductionClaim(val) {
    if (productionClaims[val]) {
        return productionClaims[val];
    } else {
        return '';
    }
}

function formatSeafoodFreshOrFrozenType(val) {
    if (seafoodFreshOrFrozenTypes[val]) {
        return seafoodFreshOrFrozenTypes[val];
    } else {
        return '';
    }
}

function formatSeafoodCatchType(val) {
    if (seafoodCatchTypes[val]) {
        return seafoodCatchTypes[val];
    } else {
        return '';
    }
}

function formatGlutenFreeAgency(val) {
    if (glutenFreeAgencies[val]) {
        return glutenFreeAgencies[val];
    } else {
        return '';
    }
}

function formatKosherAgency(val) {
    if (kosherAgencies[val]) {
        return kosherAgencies[val];
    } else {
        return '';
    }
}

function formatNonGmoAgency(val) {
    if (nonGmoAgencies[val]) {
        return nonGmoAgencies[val];
    } else {
        return '';
    }
}

function formatOrganicAgency(val) {
    if (organicAgencies[val]) {
        return organicAgencies[val];
    } else {
        return '';
    }
}

function formatVeganAgency(val) {
    if (veganAgencies[val]) {
        return veganAgencies[val];
    } else {
        return '';
    }
}

$(function () {
    loadAnimalWelfareRatings();
    loadMilkTypes();
    loadEcoScaleRatings();
    loadSeafoodFreshOrFrozenTypes();
    loadSeafoodCatchTypes();
    loadGlutenFreeAgencies();
    loadKosherAgencies();
    loadNonGmoAgencies();
    loadOrganicAgencies();
    loadVeganAgencies();
});