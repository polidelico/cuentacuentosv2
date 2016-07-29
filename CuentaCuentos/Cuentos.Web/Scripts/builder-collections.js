/* --------------------------------------------
|  Pages Collection
----------------------------------------------*/
App.Collections.Pages = Backbone.Collection.extend({
    model: App.Models.Page,
    storyId: storyId, //global variable
    url: function () {
        return '/pages/getpages/' + this.storyId;
    },
    initialize: function () {
        _.extend(App.Collections.Pages, Backbone.Events);
    },
    savePages: function () {

        var collectionInstance = this;
        this.previousState = this;

        $.ajax({
            type: "POST",
            url: '/pages/savepages/' + App.Pages.storyId,
            data: { jsonStr: JSON.stringify(App.Pages) },
            success: function (response)
            {           
                collectionInstance.trigger("saveDone", response);
                collectionInstance.status = response;
            }
        });
    },
    backupModel: null
});

/* --------------------------------------------
|  Images Collection
----------------------------------------------*/
App.Collections.Images = Backbone.Collection.extend({
    model: App.Models.Image,
    search: function (opts) {
        var result = this.where(opts);
        var resultCollection = new App.Collections.Images(result);

        return resultCollection;
    },
    url: '/pages/getimages',
});

/* --------------------------------------------
|  PageTypes Collection
----------------------------------------------*/
App.Collections.PageTypes = Backbone.Collection.extend({
    model: App.Models.PageType,
});

/* --------------------------------------------
|  Categories Collection
----------------------------------------------*/
App.Collections.Categories = Backbone.Collection.extend({
    model: App.Models.Category,
});

/* --------------------------------------------
|  Grades Collection
----------------------------------------------*/
App.Collections.Grades = Backbone.Collection.extend({
    model: App.Models.Grade,
});

/* --------------------------------------------
|  Interests Collection
----------------------------------------------*/
App.Collections.Interests = Backbone.Collection.extend({
    model: App.Models.Interest,
});

/* --------------------------------------------
|  ImageCategories Collection
----------------------------------------------*/
App.Collections.ImageCategories = Backbone.Collection.extend({
    model: App.Models.ImageCategory,
});
