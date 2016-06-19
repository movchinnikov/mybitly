app.controller('myLinksCtrl', function ($scope, $cookies, notify, cfpLoadingBar, clipboard, homeFactory) {

    cfpLoadingBar.start();
    var cookieName = 'anon_shortlinks';

    $scope.links = [];
    $scope.loadComplete = false;

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
                $scope.loadComplete = true;
                cfpLoadingBar.complete();
            })
            .error(function(ex) {
                cfpLoadingBar.complete();
                $scope.loadComplete = true;
                notify({ message: ex.data.message, duration: 3000, classes: 'alert alert-danger', position: 'right' });
            });
    } else {
        cfpLoadingBar.complete();
        $scope.loadComplete = true;
    }

    $scope.copySuccess = function (url) {
        clipboard.copyText(url);
        notify({ message: 'Ссылка успешно скопирована', duration: 3000, classes: 'alert alert-success', position: 'right' });
    }

    $scope.copyFail = function (err) {
        notify({ message: err, duration: 3000, classes: 'alert alert-success', position: 'right' });
    }
});