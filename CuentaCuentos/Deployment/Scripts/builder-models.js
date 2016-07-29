/* --------------------------------------------
|  Story Model
----------------------------------------------*/
App.Models.Story = Backbone.Model.extend({
    id: storyId, //global variable
    defaults: {
        Name: '',
        Summary: '',
        AllCategories: [],
        Categories: [],
        AllGrades: [],
        Grades: [],
    },
    url: function () {
        return '/stories/getstory/' + this.id;
    }

});

/* --------------------------------------------
|  Page Model
----------------------------------------------*/
App.Models.Page = Backbone.Model.extend({
    defaults: {
        Type: '',
        Text: '',
        ImageId: 0,
        ImageUrl: '',
        Position: 0
    }
});

/* --------------------------------------------
|  Image Model
----------------------------------------------*/
App.Models.Image = Backbone.Model.extend({});

/* --------------------------------------------
|  PageType Model
----------------------------------------------*/
App.Models.PageType = Backbone.Model.extend({});

/* --------------------------------------------
|  Category Model
----------------------------------------------*/
App.Models.Category = Backbone.Model.extend({});

/* --------------------------------------------
|  Grade Model
----------------------------------------------*/
App.Models.Grade = Backbone.Model.extend({});

/* --------------------------------------------
|  Interest Model
----------------------------------------------*/
App.Models.Interest = Backbone.Model.extend({});

/* --------------------------------------------
|  ImageCategory Model
----------------------------------------------*/
App.Models.ImageCategory = Backbone.Model.extend({});