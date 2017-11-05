function UserData() {
    this.inputType = ko.observable("Интерфейс");
    this.fieldType = ko.observable("GF(p)");
    
    this.modulus = ko.observable("");
    this.reductionPolynomial = ko.observable("");
    
    this.ellipticCurveA = ko.observable("");
    this.ellipticCurveB = ko.observable("");
    
    this.operations = ko.observableArray();
    
    this.process = ko.observable(false);
    this.result = ko.observable("");
    this.resultToDownload = ko.observable("");
    
    var self = this;
    this.isUIInputType = ko.computed(function() { return self.inputType() === "Интерфейс" });
    this.isFileInputType = ko.computed(function() { return self.inputType() === "Файл" });
    this.isPrimeField = ko.computed(function() { return self.fieldType() === "GF(p)" });
    this.isBinaryField = ko.computed(function() { return self.fieldType() === "GF(2^m)" });
    this.hasOperations = ko.computed(function () { return self.operations().length > 0 });
    this.hasResult = ko.computed(function () { return self.result() !== "" });
    this.isResultError = ko.computed(function () { return self.result().lastIndexOf("Ошибка!", 0) === 0});

    self.addOperation = function() {
        self.operations.push({ 
            x1: ko.observable(""), 
            y1: ko.observable(""), 
            
            x2: ko.observable(""), 
            y2: ko.observable(""), 
            
            factor: ko.observable(""), 

            type: ko.observable("+") })
    };

    self.removeOperation = function() {
        self.operations.remove(this);
    };
    
    self.changeType = function () {
        this.type((this.type() === '+') ? 'x' : '+');
    };
}
var data = new UserData();

function setInputType(type) {
    data.inputType(type);
    cleanResult();
}

function setFieldType(type) {
    data.fieldType(type);
}

function calculate() {
    data.process(true);
    cleanResult();

    var finiteField = { Modulus: data.modulus(), ReductionPolynomial: data.reductionPolynomial(), Type: data.fieldType() };
    var ellipticCurve = { A: data.ellipticCurveA(), B: data.ellipticCurveB() };
    var operations = $.map(data.operations(), convertOperationToString);
    $.ajax({
        type: "POST",
        async: true,
        url: "/EllipticCurves/Calculate",
        cache: false,
        data: {
            finiteField: JSON.stringify(finiteField),
            ellipticCurve: JSON.stringify(ellipticCurve),
            operations: JSON.stringify(operations)
        },
        success: function (result) {
            saveResult(result);
        },
        error: function () {
            saveResult("Ошибка! Возможно, данные введены некорректно.");
        },
        complete: function () {
            data.process(false);
        }
    });
}

function convertOperationToString(operation) {
    return { 
        X1: operation.x1(),
        Y1: operation.y1(),
        
        X2: operation.x2(),
        Y2: operation.y2(),
        
        Factor: operation.factor(),

        Type: operation.type() };
}

function loadFile() {
    if (!window.File || !window.FileReader || !window.FileList || !window.Blob) {
        alert('Этот браузер не поддерживает чтение из файлов');
        return;
    }

    var input = document.getElementById('fileInput');
    var fileName = input.files[0];

    if (!fileName || fileName === "")
        return;
    
    data.process(true);
    cleanResult();
    
    data.fr = new FileReader();
    data.fr.onload = receivedText;

    data.fr.readAsText(fileName);
}

function receivedText() {
    data.process(true);
    cleanResult();

    $.ajax({
        type: "POST",
        async: true,
        url: "/EllipticCurves/CalculateFromFile",
        cache: false,
        data: {
            finiteFieldType: data.fieldType(),
            inputFile: data.fr.result
        },
        success: function (result) {
            saveResult(result);
        },
        error: function () {
            saveResult("Ошибка! Возможно, данные введены некорректно.");
        },
        complete: function () {
            data.process(false);
        }
    });
}

function cleanResult() {
    data.result("");
    data.resultToDownload("");
}

function saveResult(result) {
    data.resultToDownload(result);
    data.result(result.replace(/\n\r?/g, "<br>"));    
}

function downloadResult() {
    var blob = new Blob([data.resultToDownload()], {type: 'text/plain'});
    
    if (window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveBlob(blob, "result.txt");
    }
    else {
        var elem = window.document.createElement('a');
        elem.href = window.URL.createObjectURL(blob);
        elem.download = "result.txt";
        document.body.appendChild(elem);
        elem.click();
        document.body.removeChild(elem);
    }
}