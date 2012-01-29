
function isArray(obj) {
    return ((typeof (obj) == "object") && isDefined(obj.length));
};

function isDefined(obj) {
    return obj && (typeof (obj) != 'undefined');
};

function each(obj, action) {
    var self = this;
    if (obj.prototype && obj.prototype == Array) {
        for (var i = 0; i < obj.length; i++) {
            action.call(self, obj[i], i, obj);
        }
    }
    else if (typeof (obj) == "object") {
        for (var name in obj) {
            if (obj.hasOwnProperty(name)) {
                action.call(self, obj[name], name, obj);
            };
        };
    }
    else {
        throw new Error("cannot iterate supplied obj");
    };
};

function normalizeKey(key) {
    if (key.indexOf("#.") > -1 || key.indexOf("#/") > -1) {
        key = key.substring(2);
    };
    return key;
};


function resolveType(property, applyFormat) {
    var propertyType;
    var nullable = false;
    var propIsArray = false;
    if (property.type == "array") {
        propIsArray = true;
        if (!isDefined(property.items)) {
            throw new Error("items not specified for array type");
        }
        // only support homogenous arrays, so .items should have 1 element
        if (property.items.length != 1) {
            throw new Error("only homogenous arrays supported");
        };
        propertyType = property.items[0];
    }
    else if (isArray(property.type)) {
        // a nullable type will look like this
        // "type": ["null","integer"]
        var errMessage;
        // check for null
        if (property.type.length == 1 || property.type.length == 2) {
            each(property.type, function (value) {
                if (value == "null") {
                    nullable = true;
                }
                else {
                    propertyType = value;
                }
            });
        }
        else {
            errMessage = "only nullable union types implemented ('null' plus one simple type)";
        }
        if (errMessage) {
            throw new Error(errMessage);
        }
    }
    else {
        propertyType = property.type;
    }
    if (propertyType.$ref) {
        propertyType = normalizeKey(propertyType.$ref);
    }
    else {
        propertyType = applyFormat(propertyType, property);
    }
    return propertyType + (nullable ? "?" : "") + (propIsArray ? "[]" : "");
}
exports.isArray = isArray;
exports.isDefined = isDefined;
exports.each = each;
exports.normalizeKey = normalizeKey;
exports.resolveType = resolveType;