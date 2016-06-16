app.controller('homeCtrl', function ($scope, homeFactory) {
    
    $scope.shorten = function() {
        homeFactory.shorten($scope.longUrl)
            .success(function(response) {
                console.log(response);
            })
        .error(function (ex) {
            console.log(ex);
            console.log(ex);
        });

    }

});