﻿var auth2 = auth2 || {};

var helper = (function () {
  return {
    onSignInCallback: function (authResult) {
      if (authResult['access_token']) {
        var accessToken = authResult['access_token'];
        var urlService = new UrlService(window.applicationInfo.urlInfo);
        var absoluteUrl = urlService.getAbsoluteUrl('api/user/activate/googleplus');
        $.ajax({
          type: 'POST',
          url: absoluteUrl,
          contentType: 'application/octet-stream; charset=utf-8',
          success: function() {
            window.location.assign(urlService.getAbsoluteUrl('dashboard'));
          },
          processData: false,
          data: accessToken
        });
      }
    },
    connectServer: function (code) {
      var urlService = new UrlService(window.applicationInfo.urlInfo);
      var absoluteUrl = urlService.getAbsoluteUrl('api/user/login/googleplus');
      $.ajax({
        type: 'POST',
        url: absoluteUrl,
        contentType: 'application/octet-stream; charset=utf-8',
        success: function () {},
        processData: false,
        data: code
      });
    }
  };
})();

function startApp() {
  gapi.load('auth2', function () {
    gapi.auth2.init({
      client_id: '34588642309-ogg3uciumd17cfu6l1m6an3hhto90543.apps.googleusercontent.com',
      cookiepolicy: 'single_host_origin',
      fetch_basic_profile: false,
      scope: 'https://www.googleapis.com/auth/plus.login'
    }).then(function () {
      auth2 = gapi.auth2.getAuthInstance();
    });
  });

  gapi.signin2.render("googleSignInButton", {
    width: 300,
    height: 50,
    longtitle: true,
    theme: 'dark',
    onsuccess: handleLoginSuccess,
    onfailure: handleLoginFailure
  });
}

// Comment to make the app NOT sign in immediately when opening page, when you are logged in at Google with your browser
function signInClick() {
  auth2.grantOfflineAccess().then(
    function (result) {
      helper.connectServer(result.code);
    });
}

function onSignInCallback(authResult) {
  helper.onSignInCallback(authResult);
}

function handleLoginSuccess(googleUser) {
  helper.onSignInCallback(googleUser.getAuthResponse());
}

function handleLoginFailure() {
  console.log('Login failure!');
}
