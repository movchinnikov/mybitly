app.config(function($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Content/js/app/home/templates/index.html',
            controller: 'homeCtrl'
        })
        .when('mylinks', {
            templateUrl: 'Content/js/app/home/templates/mylinks.html',
            controller: 'myLinksCtrl'
        });
});