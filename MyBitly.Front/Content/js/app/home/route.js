app.config(function($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Content/js/app/home/templates/index.html',
            controller: 'homeCtrl'
        });
});