app.factory('homeFactory', function ($http) {
    var service = {};

    service.shorten = function (longUrl) {
        return $http({
            url: '/api/shorten',
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            params: { longUrl: longUrl }
        });
    }

    service.linkHistory = function (hashes) {

        var request = {
            "hashes[]": hashes
        }

        return $http({
            url: '/api/linkHistory',
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            params: request
        });
    }

    return service;
});