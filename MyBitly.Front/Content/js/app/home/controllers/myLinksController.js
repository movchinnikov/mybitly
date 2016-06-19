app.controller('myLinksCtrl', function ($scope, $cookies, notify, homeFactory) {
    var cookieName = 'anon_shortlinks';

    $scope.links = [];

    var currentCookieValue = $cookies.get(cookieName);

    if (!!currentCookieValue) {
        var filter = [];
        var hashes = currentCookieValue.split(';');
        for (var i = 0; i < hashes.length; i++) {
            if (!!hashes[i]) {
                filter.push(hashes[i]);
            }
        }

        homeFactory.linkHistory(filter)
            .success(function(response) {
                $scope.links = response.data;
            })
            .error(function(ex) {
                notify({ message: ex.data.message, duration: 3000, classes: 'alert alert-danger', position: 'right' });
            });
    }
});