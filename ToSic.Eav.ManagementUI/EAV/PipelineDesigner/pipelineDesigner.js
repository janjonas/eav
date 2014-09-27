var pipelineDesigner = angular.module('pipelineDesinger', ['pipelineDesinger.filters', 'ngResource']);

pipelineDesigner.config(['$locationProvider', function ($locationProvider) {
	$locationProvider.html5Mode(true);
}]);

// datasource directive makes an element a DataSource with jsPlumb
pipelineDesigner.directive('datasource', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.makeDataSource(scope.dataSource, element, attrs);
            if (scope.$last === true) {
                scope.$emit('ngRepeatFinished');
            }
        }
    }
});

// Filters for "ClassName, AssemblyName"
angular.module('pipelineDesinger.filters', []).filter('typename', function () {
	return function (input, format) {
		var globalParts = input.match(/[^,\s]+/g);

		switch (format) {
			case 'classfqn':
				if (globalParts)
					return globalParts[0];
			case 'class':
				if (globalParts) {
					var classfqn = globalParts[0].match(/[^\.]+/g);
					return classfqn[classfqn.length - 1];
				}
		}

		return input;
	};
});