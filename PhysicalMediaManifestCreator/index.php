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

    <div class="container text-center">
        <div class="row">
            <div class="col">
                <div style="text-align: center;">
                    <br />
                    <img id="gamePreview" src="https://placehold.co/256x256" alt="Logo" width="256" height="256" style="width: 256px; height:256px;">
                </div>
                <br />
                <div class="mb-3">
                    <label for="gameID" class="form-label">Game ID</label>
                    <input type="text" class="form-control json-data" id="gameID" placeholder="Game ID">
                </div>
                <br />
                <div class="mb-3">
                    <label for="gameName" class="form-label">Game Name</label>
                    <div style="display: grid; grid-auto-flow: column; gap: 10px; grid-template-columns: 3fr 1fr;">
                        <input type="text" class="form-control json-data" id="gameName" placeholder="Game Name">
                        <button class="btn btn-primary" onclick="searchForGameIcon();">Search</button>
                    </div>
                </div>
                <br />
                <div class="mb-3">
                    <label for="gamePath" class="form-label">Game Path</label>
                    <div style="display: grid; grid-auto-flow: column; gap: 10px; grid-template-columns: 3fr 1fr;">
                        <input type="text" class="form-control json-data" id="gamePath" placeholder="Game Path">
                        <button class="btn btn-primary" onclick="selectGameExe();">Select Path</button>
                    </div>
                </div>
                <br />
                <div class="mb-3">
                    <label for="args" class="form-label">Game Arguments</label>
                    <input type="text" class="form-control json-data" id="args" placeholder="Game Arguments">
                </div>
            </div>
            <div class="col" style="margin-top: 20px;">
                <div id="searchResults">

                </div>
            </div>
        </div>
    </div>


    <script>
        function id(elemID) {
            return $(`#${elemID}`)[0];
        }

        async function getAllExecutables(dirHandle, parent, starting_dir) {
            const files = [];
            for await (let [name, handle] of dirHandle) {
                if (handle.kind === 'directory') {
                    files.push(...await getAllExecutables(handle, `${parent}/${name}`, starting_dir));
                } else {
                    if (name.endsWith(".exe")) {
                        files.push(`${parent}/${name}`.substring(starting_dir.length + 1));
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

        async function selectGameExe() {
            try {
                const directoryHandle = await window.showDirectoryPicker();
                const starting_dir = directoryHandle.name;
                const executables = await getAllExecutables(directoryHandle, starting_dir, starting_dir);
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
            id("gamePreview").src = img.src;
        }
    </script>
</body>

</html>