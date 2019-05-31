var App = App || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('DynamicWebApi');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

})(App);