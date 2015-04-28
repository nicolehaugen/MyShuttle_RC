var MyShuttle = MyShuttle || {};

MyShuttle.Config = function(){
    var bingMapsKey = 'AowwaZssHfABfk67j7II30OYz2E4PF2qYsX3kSDLjokOyDLFR3HBozSlZY9gNb6e',
        infobBoxCompanyAddress = 'Spring Studios, 164 E Chicago Ave',
        companyLocation = {
            Latitude: 41.896755,
            Longitude: -87.623470
        },
        smallScreenMinWidth = 768;

    return {
        bingMapsKey: bingMapsKey,
        infoBoxCompanyAddress: infobBoxCompanyAddress,
        companyLocation: companyLocation,
        smallScreenMinWidth: smallScreenMinWidth
    }
}();