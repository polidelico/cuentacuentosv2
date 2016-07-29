/* --------------------------------------------
|  App View
----------------------------------------------*/

App.Views.App = Backbone.View.extend({
    initialize: function () {

        //get general data 
        App.Categories = new App.Collections.Categories(App.CategoriesJson);
        App.Grades = new App.Collections.Grades(App.GradesJson);
        App.PageTypes = new App.Collections.PageTypes(App.PageTypesJson);

        //set story properties
        App.Story.set('AllCategories', App.Categories);
        App.Story.set('AllGrades', App.Grades);
        
        // instantiate views
        
        var saveStoryButtonView = new App.Views.SaveStoryButton();
        var addPageButtonView = new App.Views.AddPageButton();
        var storyInfoModalView = new App.Views.StoryInfoModal({model: App.Story}); // doing
        var pageTypesModalView = new App.Views.PageTypesModal({ collection: App.PageTypes });
        var pagesView = new App.Views.Pages({ collection: this.collection }).render();
        var editorView = new App.Views.Editor({ collection: this.collection.slice(0, 1) }).render();

        // Modals
        $('#template-selector-modal').easyModal();
        $('#story-details-modal').easyModal();

        //
        App.vent.on('addPage', this.addPage, this)
    },
    addPage: function (pageTypeModel) {

        this.collection.add({
            Type: pageTypeModel.get('Name'),
            Text: '<h2> Tu texto va aqui </h2>',
            ImageId: 0,
            ImageUrl: 'http://placehold.it/250x200&text=cambiar%20imagen',
            Position: this.collection.length
        });

        App.Pages.savePages();
    }

});


/* --------------------------------------------
|  Pages View - Navigation
----------------------------------------------*/
App.Views.Pages = Backbone.View.extend({
    el: '#pages-nav ul.pages',
    initialize: function () {
        this.collection.on('add', this.addOne, this);
        this.collection.on('destroy', this.deletePage, this);
        this.collection.on('change', this.modelChange, this);
        this.collection.on('saveDone', this.validateSave, this);
    },
    render: function () {
        
        this.collection.each(this.addOne, this);
        return this;

    },    
    addOne: function (page) {

        var pageView = new App.Views.Page({ model: page });
        this.$el.append(pageView.render().el);

    },
    deletePage: function (object) {

        this.collection.savePages();

    },
    modelChange: function () {

        this.collection.savePages();
        this.$el.empty();
        this.render();
    },
    validateSave: function (status) {

        if (!status)
        {
            alert('Oops, something went wrong, please try again');
            //TODO: reload the collection
            location.reload();
        }
        else {
            //console.log('jobs done');
        }
    }
});

/* --------------------------------------------
|  Page View - Navigation
----------------------------------------------*/
App.Views.Page = Backbone.View.extend({
    tagName: 'li',
    template: getTemplate('pages-nav-template'),
    initialize: function () {
        this.model.on('destroy', this.remove, this);
        this.model.on('change', this.test, this);
    },
    test: function () {
        this.render();
        //console.log(this);
    },
    events: {
        'click a.page' : 'bringToEditor',
        'click .delete': 'deletePage'        
    },
    bringToEditor: function (e) {

        e.preventDefault();
        var index = App.Pages.indexOf(this.model);
        var editCollection;

        if (index == 0 || index % 2 == 0) {
            editCollection = index == 0 ? App.Pages.slice(index, index + 1) : App.Pages.slice(index - 1, index + 1);
        } else {
            editCollection = App.Pages.slice(index, index + 2);            
        }

        var editorView = new App.Views.Editor({ collection: editCollection }).render();

    },
    deletePage: function () {
        this.model.destroy();
        //this.model.trigger('destroy', this);
    },    
    render: function () {

        var source = Handlebars.compile(this.template);
        this.setElement(source(this.model.toJSON()));
        return this;

    }

});


/* --------------------------------------------
|  Editor View
----------------------------------------------*/
App.Views.Editor = Backbone.View.extend({
    el: '#pages-designer ul',
    initialize: function () {

    },
    render: function () {

        //console.log(this.collection);
        this.$el.empty();
        _.each(this.collection, this.addOne, this);

        CKEDITOR.disableAutoInline = true;
        $('#pages-designer div[contenteditable="true"]').ckeditor();
        return this;

    },
    addOne: function (page, index) {

        var cssClass = 'fleft';
        var editorPage = new App.Views.EditorPage({ model: page });

        if (this.collection.length == 1 || index == 1)
            cssClass = 'fright';

        editorPage.positionClass = cssClass;
        this.$el.append(editorPage.render().el);
    }
});

/* --------------------------------------------
|  EditorPage View
----------------------------------------------*/
App.Views.EditorPage = Backbone.View.extend({
    template: getTemplate('pages-designer-page'),
    events: {
        'blur div[contenteditable="true"]' : 'saveText'
    },
    
    render: function () {

        var source = Handlebars.compile(this.template);
        this.setElement(source({ page: this.model.toJSON(), cssclass: this.positionClass }));
        return this;
    },
    saveText: function () {

        var newText = this.$('div[contenteditable="true"]').html();
        this.model.set('Text', newText);
        //console.log(this.model);

    }
});

/* --------------------------------------------
|  SaveStoryButton View (OpenModal)
----------------------------------------------*/
App.Views.SaveStoryButton = Backbone.View.extend({
    el: '#save-story',
    events: {
        'click': 'openModal'
    },
    openModal: function (e) {
        e.preventDefault();
        $('#story-details-modal').trigger('openModal');
    }
});

/* --------------------------------------------
|  AddPageButton View (OpenModal)
----------------------------------------------*/
App.Views.AddPageButton = Backbone.View.extend({
    el: '#add-page',
    events: {
        'click': 'openModal'
    },
    openModal: function (e) {
        e.preventDefault();
        $('#template-selector-modal').trigger('openModal');
    }
});

/* --------------------------------------------
|  StoryInfoModal View
----------------------------------------------*/
App.Views.StoryInfoModal = Backbone.View.extend({
    el: '#story-details-modal',
    template: getTemplate('story-details'),
    events:{
        'click #submit-details': 'saveStory'
    },
    initialize: function () {
        this.render();
    },
    render: function () {

        var source = Handlebars.compile(this.template);
        var html = source({
            story: this.model.toJSON(),
            allCategories: this.model.get('AllCategories').toJSON(),
            allGrades: this.model.get('AllGrades').toJSON()
        });
        this.$el.html(html);
        console.log(this.model.toJSON());

    },
    saveStory: function (e) {

        e.preventDefault();

        var selCategories = [];
        var selGrades = [];
        var name = this.$('#story-name').val();
        var summary = this.$('#story-summary').val();
        var categories = this.$('input[name="category"]:checked');
        var grades = this.$('input[name="grade"]:checked');

        categories.each(function (i, e) {
            selCategories[i] = $(this).val();
        });
        grades.each(function (i, e) {
            selGrades[i] = $(this).val();
        });

        //TODO: Mover fotos al folder de fotos para el cuento

        $.ajax({
            type: "POST",
            url: '/stories/save/' + App.Pages.storyId,
            data: {
                name: name,
                summary: summary,
                selectedCategories: JSON.stringify(selCategories),
                selectedGrades: JSON.stringify(selGrades)
            },
            success: function (response) {
                alert('Guardado');
                $('#story-details-modal').trigger('closeModal');
            },
            error: function (response) {

            }
        });

        //var itemId = $(e.currentTarget).data('id');
        //var itemModel = this.collection.at(itemId);
        //App.vent.trigger('addPage', itemModel);
        //$('#template-selector-modal').trigger('closeModal');

    }
});

/* --------------------------------------------
|  AddPageModal View
----------------------------------------------*/
App.Views.PageTypesModal = Backbone.View.extend({
    el: '#template-selector-modal',
    template: getTemplate('pages-type-list'),
    events: {
        'click ul li': 'addPage'
    },
    initialize: function () {
        this.render();
    },
    render: function () {

        var source = Handlebars.compile(this.template);
        var html = source({ pageTypes: this.collection.toJSON() });
        this.$el.html(html);

    },
    addPage: function (e) {

        e.preventDefault();

        var itemId = $(e.currentTarget).data('id');
        var itemModel = this.collection.at(itemId);
        App.vent.trigger('addPage', itemModel);
        $('#template-selector-modal').trigger('closeModal');

    }
});