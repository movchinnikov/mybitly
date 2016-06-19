app.controller('homeCtrl', function ($scope, $cookies, notify, homeFactory) {
    var cookieName = 'anon_shortlinks';

    $scope.shorten = function () {
        $scope.hash = "";
        $scope.shortUrl = undefined;

        homeFactory.shorten($scope.longUrl)
            .success(function (response) {
                $scope.shortUrl = response.data.short_url;
                var currentCookieValue = $cookies.get(cookieName);
                var responseCookie = response.data.hash + ';';
                var newCookieValue = ((!!currentCookieValue) ? currentCookieValue : '') + responseCookie;
                $cookies.put(cookieName, newCookieValue);

                notify({ message: 'Ссылка успешно создана', duration: 3000, classes: 'alert alert-success', position: 'right' });
            })
        .error(function (ex) {
            notify({ message: ex.data.message, duration: 3000, classes: 'alert alert-danger', position: 'right' });
        });

    }
});