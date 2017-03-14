"use strict";
var UrlService = (function () {
  function UrlService(urlInfo) {
    if (!urlInfo) throw new Error("No urlInfo provided");
    this.siteUrl = UrlService.trimSlashes(urlInfo.siteUrl || "");
    this.baseUrl = UrlService.trimSlashes(urlInfo.baseUrl || "");
  }

  /**
   * Converts a relative URL (e.g. /api/some-resource)
   * into an absolute URL (e.g. http://thehospital.com/ultragendabroka/api/some-resource)
   * @param relativeUrl {string} the relative url. Leading slashes are ignored, if any.
   * @returns {string} an absolute URL (e.g. http://thehospital.com/ultragendabroka/api/some-resource)
   */
  UrlService.prototype.getAbsoluteUrl = function (relativeUrl) {
    // already an absolute URL, no need to do anything
    if (relativeUrl.indexOf("http") === 0) {
      return relativeUrl;
    }
    /*
     combine
     - the site url (http://thehospital.com)
     - the base url (/UltraGendaBroka)
     - the relative url (api/some-resource)
     */
    return [this.siteUrl, this.baseUrl, UrlService.trimSlashes(relativeUrl)]
        .filter(function (_) { return _; }) // only non-empty fragments
        .join('/');
  };

  /**
   * Removes the leading and trailing slashes (forward or backward) from a string
   * @param string
   * @returns {string} a string with no leading or trailing slashes (forward or backward)
   */
  UrlService.trimSlashes = function (string) {
    return string.replace(/^\/+|\/+$/g, '');
  };
  return UrlService;
}());