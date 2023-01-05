export function invokeApi(method, callback, wrapper) {
    wrapper.invokeMethodAsync('GetStatus', 'running...');
    if (window._spidereye != null) {
        window._spidereye.invokeApi(method, null,
            e => {
                wrapper.invokeMethodAsync(callback, e);
                console.log('done!', e);
            });
        return "waiting...";
    } else {
        return "This environment is not supported!";
    }
};

export function showWindow(wrapper) {
    if (window._spidereye != null) {
        window._spidereye.invokeApi("f0631cfea99a_Window.show", windowConfig, e => {
            wrapper.invokeMethodAsync('GetResult', e);
            if (e.success) {
                if (result != null) {
                    console.log("success");
                    return "success";
                }
            } else if (error != null) {
                console.log(e.error);
                return "Error:"+ e.error;
            }
        });
        return "Error:Something wrong.";
    } else {
        return "This environment is not supported!";
    }
}

const windowConfig = {
    title: 'Hello World',
    width: 900,
    height: 600,
    minWidth: 0,
    minHeight: 0,
    maxWidth: 0,
    maxHeight: 0,
    backgroundColor: '#303030',
    canResize: true,
    useBrowserTitle: true,
    enableScriptInterface: true,
    //enableDevTools: true,
    url: 'http://localhost:5000'
};