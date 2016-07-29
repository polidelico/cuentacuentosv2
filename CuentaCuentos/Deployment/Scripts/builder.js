(function () {

    window.App = {
        Models: {},
        Collections: {},
        Views: {}
    };

    window.getTemplate = function (id) {
        return $('#' + id).html();
    };

    App.vent = _.extend({}, Backbone.Events);

})();