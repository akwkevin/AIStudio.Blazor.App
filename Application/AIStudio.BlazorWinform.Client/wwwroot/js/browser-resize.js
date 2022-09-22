window.getDimensions = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.AddResize = function (methodName, dot) {
    window.onresize = function () {
        try {
            dot.invokeMethodAsync(methodName);
        } catch { }
    };
};