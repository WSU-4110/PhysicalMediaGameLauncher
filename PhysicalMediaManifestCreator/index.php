<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Physical Media Creator</title>
    <script src="./jquery.js"></script>
    <script src="./js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="./css/bootstrap.min.css">
    <style>
        .steamdb-img-preview {
            width: 128px;
            height: 128px;
            border: 2px solid transparent;
            border-radius: 10px;
        }

        .hide {
            display: none;
        }
    </style>
</head>

<body>

    <div class="modal" tabindex="-1" id="exeModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Select an Executable</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body" id="exectuableSelections" style="display: grid; grid-auto-flow: row; gap: 10px;"></div>
            </div>
        </div>
    </div>

    <ul class="nav" style="border-bottom: 2px solid black;">
        <li class="nav-item">
            <a class="nav-link active" aria-current="page" href="javascript:void(0);" onclick="loadFromDevice();">Load from Drive</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" href="javascript:void(0);" onclick="saveToDevice();">Save to Device</a>
        </li>
    </ul>

    <ul class="nav nav-tabs" id="gamesTabs">
        <!-- <li class="nav-item">
            <a class="nav-link active">Active</a>
        </li> -->
        <li class="nav-item">
            <a class="nav-link" href="javascript:void(0);" onclick="addGame();">+</a>
        </li>
    </ul>

    <div class="container text-center hide" id="gameDataForm">
        <div class="row">
            <div class="col">
                <div style="text-align: center;">
                    <br />
                    <img id="gamePreview" src="https://placehold.co/256x256" alt="Logo" width="256" height="256" style="width: 256px; height:256px;">
                </div>
                <br />
                <div class="mb-3">
                    <label for="id" class="form-label">Game ID</label>
                    <input type="text" class="form-control json-data" id="id" placeholder="Game ID" oninput="updateValue(this);" onchange="updateValue(this);">
                </div>
                <br />
                <div class="mb-3">
                    <label for="gameName" class="form-label">Game Name</label>
                    <div style="display: grid; grid-auto-flow: column; gap: 10px; grid-template-columns: 3fr 1fr;">
                        <input type="text" class="form-control json-data" id="gameName" placeholder="Game Name" oninput="updateValue(this);" onchange="updateValue(this);">
                        <button class="btn btn-primary" onclick="searchForGameIcon();">Search</button>
                    </div>
                </div>
                <br />
                <div class="mb-3">
                    <label for="gamePath" class="form-label">Game Path</label>
                    <div style="display: grid; grid-auto-flow: column; gap: 10px; grid-template-columns: 3fr 1fr;">
                        <input type="text" class="form-control json-data" id="gamePath" placeholder="Game Path" oninput="updateValue(this);" onchange="updateValue(this);">
                        <button class="btn btn-primary" onclick="selectGameExeFromBtn();">Select Path</button>
                    </div>
                </div>
                <br />
                <div class="mb-3">
                    <label for="args" class="form-label">Game Arguments</label>
                    <input type="text" class="form-control json-data" id="args" placeholder="Game Arguments" oninput="updateValue(this);" onchange="updateValue(this);">
                </div>
                <br />
                <div class="mb-3">
                    <label for="args" class="form-label">Game Description</label>
                    <textarea type="text" class="form-control json-data" id="gameDescription" oninput="updateValue(this);" onchange="updateValue(this);"></textarea>
                </div>
            </div>
            <div class="col" style="margin-top: 20px;">
                <div id="searchResults">

                </div>
            </div>
        </div>
    </div>


    <script>
        var games = [];
        var selected_idx = -1;

        var images_ready = [];
        var images = {};

        function addGame() {
            games.push({
                "id": ""
            });
            reloadTabs();
        }

        function updateValue(elem) {
            if (selected_idx == -1) {
                return;
            }

            games[selected_idx][elem.id] = elem.value.trim();
            if (elem.id == "id") {
                document.querySelector(`#gamesTabs li:nth-child(${selected_idx + 1}) a`).innerHTML = elem.value.trim() != "" ? elem.value.trim() : `Untitled ${selected_idx}`;
            }
        }

        function selectGame(elem, idx) {
            if (selected_idx == idx) {
                return;
            }

            selected_idx = idx;
            Array.from(document.querySelectorAll("#gamesTabs .active")).forEach(x => x.classList.remove("active"));
            elem.classList.add("active");

            id("gameDataForm").classList.remove("hide");
            Object.keys(games[selected_idx]).forEach(x => {
                let elem = id(x);
                if (elem != undefined) {
                    elem.value = games[selected_idx][x];
                }
            });
            if("img" in games[selected_idx]){
                id("gamePreview").src = games[selected_idx]["img"];
            } else {
                id("gamePreview").src = "https://placehold.co/256x256";
            }
        }

        function reloadTabs() {
            var tmp = [];
            for (var i = 0; i < games.length; i++) {
                tmp.push(`
                <li class="nav-item">
                    <a class="nav-link" href="javascript:void(0);" onclick="selectGame(this, ${i});">${games[i]["id"] == "" ? `Untitled ${i}` : games[i]["id"]}</a>
                </li>
                `);
            }

            id("gamesTabs").innerHTML = `
                ${tmp.join('\n')}
                <li class="nav-item">
                    <a class="nav-link" href="javascript:void(0);" onclick="addGame();">+</a>
                </li>
            `;
        }

        async function loadFromDevice() {
            await loadExesFromDirectory(true);
            
            var inter = setInterval(() => {
                if(images_ready.filter(x => x).length == images_ready.length) {
                    for(var i = 0; i < games.length; i++){
                        if(games[i]["id"] in images) {
                            games[i]["img"] = images[games[i]["id"]];
                        }
                    }
                    reloadTabs();
                    clearInterval(inter);
                } 
            }, 800);
        }

        async function saveToDevice() {
            try {
                let sfd = await window.showSaveFilePicker({
                    "id": "game-data-json",
                    "suggestedName": "gameData.json",
                    "types": [
                        {
                            "description": "JSON File",
                            "accept": {"application/json": [".json"]},
                        }
                    ]
                });

                let output_json = JSON.stringify({
                    "games": games.map((x) => {
                        let obj = {
                            "id": x["id"],
                            "gamePath": x["gamePath"],
                            "gameName": x["gameName"],
                            "args": x["args"],
                        };
                        obj["gameDescription"] = "gameDescription" in x ? x["gameDescription"] : "";
                        return obj;
                    })
                });

                let writeable = await sfd.createWritable();
                await writeable.write(output_json);
                await writeable.close();
                

            } catch (error) {
                console.log(error);
            }
        }

        var executables = [];


        function id(elemID) {
            return $(`#${elemID}`)[0];
        }

        async function getAllExecutables(dirHandle, parent, starting_dir, read_existing_data) {
            const files = [];
            for await (let [name, handle] of dirHandle) {
                if (handle.kind === 'directory') {
                    files.push(...await getAllExecutables(handle, `${parent}/${name}`, starting_dir, read_existing_data));
                } else {
                    if (name.endsWith(".exe")) {
                        files.push(`${parent}/${name}`.substring(starting_dir.length + 1));
                    }

                    if(read_existing_data){
                        if(name.endsWith(".png")) {
                            const file = await handle.getFile();
                            const reader = new FileReader();
                            let idx = images_ready.length;
                            images_ready.push(false);
                            reader.onload = () => {
                                console.log(idx);
                                let id = name.substring(0, name.indexOf(".")).trim();
                                images[id] = reader.result;
                                images_ready[idx] = true;
                            }
                            reader.onerror = () => {
                                images_ready[idx] = true;
                            }
                            reader.onabort = () => {
                                images_ready[idx] = true;
                            }
                            reader.readAsDataURL(file);
                        }
    
                        if(name == "gameData.json" && parent == "\\"){
                            console.log(`${name}`);
                            const file = await handle.getFile();
                            const reader = new FileReader();
                            reader.onload = () => {
                                let data = JSON.parse(reader.result);
                                games = data["games"];
                            }
                            reader.readAsText(file);
                        }
                    }
                }
            }
            return files;
        }

        function showExes(exes) {
            let exesBtns = [];
            exes.forEach(x => {
                exesBtns.push(`<button class="btn btn-success" onclick="reportPath('${x}')">${x}</button>`);
            });
            id("exectuableSelections").innerHTML = exesBtns.join('\n');
            $("#exeModal").modal("show");
        }

        async function loadExesFromDirectory(read_existing_data) {
            const directoryHandle = await window.showDirectoryPicker();
            const starting_dir = directoryHandle.name;
            executables = await getAllExecutables(directoryHandle, starting_dir, starting_dir, read_existing_data);
        }

        async function selectGameExeFromBtn() {
            try {
                if (executables.length <= 0) {
                    await loadExesFromDirectory(false);
                }
                showExes(executables);
            } catch (error) {
                console.log(error);
            }
        }

        function reportPath(path) {
            id("gamePath").value = path;
            $("#exeModal").modal("hide");
        }

        function searchForGameIcon() {
            id("searchResults").innerHTML = "";
            let gameName = id("gameName").value;

            $.ajax({
                url: `http://localhost:8080/api.php?api=GetGameData&gameName=${gameName}`,
                dataType: "json",
                success: (resp) => {
                    if (resp["success"]) {
                        let btns = [];
                        resp["data"].forEach(x => {
                            btns.push(`<button onclick='getGameIcons(${x["id"]})' class="btn btn-success">${x["name"]}</button>`);
                        });
                        id("searchResults").innerHTML = `
                            <h2>Select a Game</h2>
                            <div style="display: grid; grid-auto-flow: row; gap: 10px; justify-content: center">
                                ${btns.join('\n')}
                            </div>
                        `;
                    }
                },
            });
        }

        function getGameIcons(gameId) {
            id("searchResults").innerHTML = "";
            console.log(gameId);
            $.ajax({
                url: `http://localhost:8080/api.php?api=GetGameIcons&gameId=${gameId}`,
                dataType: "json",
                success: (resp) => {
                    if (resp["success"]) {
                        let imgs = [];
                        resp["data"].forEach(x => {
                            imgs.push(`<img src="${x["thumb"]}" alt="${x["notes"]}" onclick="updateImage(this)" class="steamdb-img-preview" width="256" height="256" />`);
                        });
                        id("searchResults").innerHTML = `
                            <h2>Select an Icon</h2>
                            <div>
                                ${imgs.join('\n')}
                            </div>
                        `;
                    }
                },
            });

        }

        function updateImage(img) {
            $.ajax({
                url: `http://localhost:8080/api.php?api=GetDataURI&img=${img.src}`,
                success: (resp) => {
                    console.log(resp);
                    id("gamePreview").src = resp.trim();
                    games[selected_idx]["img"] = resp.trim();
                },
            });
        }
    </script>
</body>

</html>