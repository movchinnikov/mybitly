app.controller('homeCtrl', function ($scope, $cookies, notify, cfpLoadingBar, clipboard, homeFactory) {
	var cookieName = 'anon_shortlinks';

	$scope.shorten = function() {
		$scope.hash = "";
		$scope.shortUrl = undefined;
		cfpLoadingBar.start();

		homeFactory.shorten($scope.longUrl)
			.success(function(response) {
				$scope.shortUrl = response.data.short_url;
				var currentCookieValue = $cookies.get(cookieName);
				var responseCookie = response.data.hash + ';';
				var newCookieValue = ((!!currentCookieValue) ? currentCookieValue : '') + responseCookie;
				$cookies.put(cookieName, newCookieValue);

				cfpLoadingBar.complete();
				notify({ message: 'Ссылка успешно создана', duration: 3000, classes: 'alert alert-success', position: 'right' });
			})
			.error(function(ex) {
				cfpLoadingBar.complete();
				notify({ message: ex.data.message, duration: 3000, classes: 'alert alert-danger', position: 'right' });
			});
	}

	$scope.copySuccess = function() {
		clipboard.copyText($scope.shortUrl);
		notify({ message: 'Ссылка успешно скопирована', duration: 3000, classes: 'alert alert-success', position: 'right' });
	}

	$scope.copyFail = function (err) {
		notify({ message: err, duration: 3000, classes: 'alert alert-success', position: 'right' });
	}
});