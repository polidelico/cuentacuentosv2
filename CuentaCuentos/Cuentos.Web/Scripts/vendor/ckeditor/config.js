/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

	// The toolbar groups arrangement, optimized for two toolbar rows.
	config.toolbarGroups = [
		//{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
		{ name: 'paragraph', groups: ['align'] },
		{ name: 'styles', groups: ['Format'] },
		{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
		{ name: 'colors' }
       // '/',
	];

	// Remove some buttons provided by the standard plugins, which are
	// not needed in the Standard(s) toolbar.
	config.removeButtons = 'Underline,Subscript,Superscript,Styles,list,indent';

	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;h4;h5';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';

    // Extra Plugins
	config.extraPlugins = 'colorbutton,justify';

	config.forcePasteAsPlainText = true;
	config.language = 'es';
	config.removePlugins = 'magicline,tabletools,contextmenu';
};
