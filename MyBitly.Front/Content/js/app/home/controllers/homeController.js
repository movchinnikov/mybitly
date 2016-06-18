app.controller('homeCtrl', function ($scope, $cookies, homeFactory) {
    var cookieName = 'anon_shortlinks';

    $scope.shorten = function () {
        $scope.hash = "";
        $scope.shortUrl = undefined;
        homeFactory.shorten($scope.longUrl)
            .success(function (response) {
                $scope.shortUrl = response.data.short_url;
                var id = response.data.id;
                var currentCookieValue = $cookies.get(cookieName);
                var responseCookie = $scope.shortUrl + ':' + id + ';';
                var newCookieValue = ((!!currentCookieValue) ? currentCookieValue : '') + responseCookie;
                $cookies.put(cookieName, newCookieValue);
            })
        .error(function (ex) {
            console.log(ex);
        });

    }
});