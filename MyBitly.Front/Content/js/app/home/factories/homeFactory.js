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

    return service;
});