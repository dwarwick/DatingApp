window.loginPost = async function(url, data) {
    const response = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "same-origin"
    });
    return response.ok;
}

window.logoutPost = async function(url) {
    const response = await fetch(url, {
        method: "POST",
        credentials: "same-origin"
    });
    return response.ok;
}
