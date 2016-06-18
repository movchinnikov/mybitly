app.controller('homeCtrl', function ($scope, homeFactory) {
    $scope.shorten = function () {
        $scope.hash = "";
        homeFactory.shorten($scope.longUrl)
            .success(function (response) {
                $scope.shortUrl = response.data.short_url;
            })
        .error(function (ex) {
            console.log(ex);
        });

    }

});