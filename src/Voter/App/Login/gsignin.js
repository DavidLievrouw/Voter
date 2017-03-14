var auth2 = auth2 || {};

var helper = (function () {
  var authResult = undefined;

  return {
    /**
     * Hides the sign-in button and connects the server-side app after
     * the user successfully signs in.
     *
     * @param {Object} authResult An Object which contains the access token and
     *   other authentication information.
     */
    onSignInCallback: function (authResult) {
      $('#authResult').html('Auth Result:<br/>');
      for (var field in authResult) {
        $('#authResult').append(' ' + field + ': ' + authResult[field] + '<br/>');
      }
      if (authResult['access_token']) {
        // The user is signed in
        this.authResult = authResult;

        var urlService = new UrlService(window.applicationInfo.urlInfo);
        var absoluteUrl = urlService.getAbsoluteUrl('api/user/activate/googleplus');
        $.ajax({
          type: 'POST',
          url: absoluteUrl,
          contentType: 'application/octet-stream; charset=utf-8',
          success: function (result) {
            console.log(result);
            window.location.assign(urlService.getAbsoluteUrl('dashboard'));
          },
          processData: false,
          data: authResult['access_token']
        });

        // After we load the Google+ API, render the profile data from Google+.
        //gapi.client.load('plus', 'v1', this.renderProfile);
      } else if (authResult['error']) {
        // There was an error, which means the user is not signed in.
        // As an example, you can troubleshoot by writing to the console:
        console.log('There was an error: ' + authResult['error']);
        $('#authResult').append('Logged out');
        $('#authOps').hide('slow');
        $('#gConnect').show();
      }
      console.log('authResult', authResult);
    },

    /**
     * Retrieves and renders the authenticated user's Google+ profile.
     */
    renderProfile: function () {
      var request = gapi.client.plus.people.get({ 'userId': 'me' });
      request.execute(function (profile) {
        $('#profile').empty();
        if (profile.error) {
          $('#profile').append(profile.error);
          return;
        }
        $('#profile').append(
            $('<p><img src=\"' + profile.image.url + '\"></p>'));
        $('#profile').append(
            $('<p>Hello ' + profile.displayName + '!<br />Tagline: ' +
            profile.tagline + '<br />About: ' + profile.aboutMe + '</p>'));
        if (profile.cover && profile.coverPhoto) {
          $('#profile').append(
              $('<p><img src=\"' + profile.cover.coverPhoto.url + '\"></p>'));
        }
      });
      $('#authOps').show('slow');
      $('#gConnect').hide();
    },
    /**
     * Calls the server endpoint to disconnect the app for the user.
     */
    disconnectServer: function () {
      var urlService = new UrlService(window.applicationInfo.urlInfo);
      var absoluteUrl = urlService.getAbsoluteUrl('api/user/disconnect/googleplus');
      // Revoke the server tokens
      $.ajax({
        type: 'POST',
        url: absoluteUrl,
        async: false,
        success: function (result) {
          console.log('revoke response: ' + result);
          $('#authOps').hide();
          $('#profile').empty();
          $('#authResult').empty();
          $('#gConnect').show();
        },
        error: function (e) {
          console.log(e);
        }
      });
    },
    /**
     * Calls the server endpoint to connect the app for the user. The client
     * sends the one-time authorization code to the server and the server
     * exchanges the code for its own tokens to use for offline API access.
     * For more information, see:
     *   https://developers.google.com/+/web/signin/server-side-flow
     */
    connectServer: function (code) {
      console.log(code);
      var urlService = new UrlService(window.applicationInfo.urlInfo);
      var absoluteUrl = urlService.getAbsoluteUrl('api/user/login/googleplus');
      $.ajax({
        type: 'POST',
        url: absoluteUrl,
        contentType: 'application/octet-stream; charset=utf-8',
        success: function (result) {
          console.log(result);
          onSignInCallback(auth2.currentUser.get().getAuthResponse());
        },
        processData: false,
        data: code
      });
    }
  };
})();

/**
 * Called after the Google client library has loaded.
 */
function startApp() {
  gapi.load('auth2', function () {

    // Retrieve the singleton for the GoogleAuth library and setup the client.
    gapi.auth2.init({
      client_id: '34588642309-ogg3uciumd17cfu6l1m6an3hhto90543.apps.googleusercontent.com', // Dali changed
      cookiepolicy: 'single_host_origin',
      fetch_basic_profile: false,
      scope: 'https://www.googleapis.com/auth/plus.login'
    }).then(function () {
      console.log('init');
      auth2 = gapi.auth2.getAuthInstance();
      auth2.then(function () {
        onSignInCallback(auth2.currentUser.get().getAuthResponse());
      });
    }, function (err) {
      console.log(err);
    });
  });
}

function signInClick() {
  auth2.grantOfflineAccess().then(
      function (result) {
        helper.connectServer(result.code);
      });
}

function onSignInCallback(authResult) {
  helper.onSignInCallback(authResult);
}
