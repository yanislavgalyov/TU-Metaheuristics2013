Raphael(10, 10, 1000, 400, function () {
    var r = this;
    r.rect(0, 0, 1000, 400, 10).attr({
        stroke: "none",
        fill: "0-#9bb7cb-#adc8da"
    });

    r.setStart();
    var hue = Math.random();
    for (var country in worldmap.shapes) {
        r.path(worldmap.shapes[country]).attr({ stroke: "#ccc6ae", fill: "#f0efeb", "stroke-opacity": 0.25 });
    }
    var world = r.setFinish();
    world.getXY = function (lat, lon) {
        return {
            cx: lon * 2.6938 + 465.4,
            cy: lat * -2.6938 + 227.066
        };
    };
    world.getLatLon = function (x, y) {
        return {
            lat: (y - 227.066) / -2.6938,
            lon: (x - 465.4) / 2.6938
        };
    };
    var latlonrg = /(\d+(?:\.\d+)?)[\xb0\s]?\s*(?:(\d+(?:\.\d+)?)['\u2019\u2032\s])?\s*(?:(\d+(?:\.\d+)?)["\u201d\u2033\s])?\s*([SNEW])?/i;
    world.parseLatLon = function (latlon) {
        var m = String(latlon).split(latlonrg),
            lat = m && +m[1] + (m[2] || 0) / 60 + (m[3] || 0) / 3600;
        if (m[4].toUpperCase() == "S") {
            lat = -lat;
        }
        var lon = m && +m[6] + (m[7] || 0) / 60 + (m[8] || 0) / 3600;
        if (m[9].toUpperCase() == "W") {
            lon = -lon;
        }
        return this.getXY(lat, lon);
    };

    world.Xs = [];
    world.Ys = [];
    world.path = undefined;

    function Iterate() {
        $.ajax({
            type: "POST",
            url: '/Home/Iterate',
            traditional: true,
            data: { it: parseInt(iterations.value), xs: world.Xs, ys: world.Ys },
            success: function (data) {
                if (world.path) {
                    world.path.remove();
                }
                world.path = r.path(data.cmd);
                if (data.cont) {
                    Iterate();
                }
                else {
                    alert('Алгоритъмът е приключен');
                }
            },
            error: function (q, w, e) {
                alert();
            },
            dataType: 'json'
        });
    }

    $('#cities').click(function (e) {

        e = e || window.event;
        var target = e.target || e.srcElement || document,
            dot = r.circle().attr({ fill: "r#FE7727:50-#F57124:100", stroke: "#fff", "stroke-width": 2, r: 0 });

        if (target.tagName == "A") {  // check if tag is A href
            var txt = decodeURIComponent(target.href.substring(target.href.indexOf("#") + 1)),
                attr = world.parseLatLon(txt);

            world.Xs.push(attr.cx);
            world.Ys.push(attr.cy);

            dot.stop().attr(attr).animate({ r: 5 }, 1000, "elastic");

            target.style.display = 'none';
            $('#citiesNum').val(parseInt($('#citiesNum').val()) + 1);

            return false;
        }
    });

    $('#reset').click(function () {
        location.reload();
    });

    $('#random').click(function () {
        var rx,
            ry,
            i,
            attr,
            dot = r.circle().attr({ fill: "r#FE7727:50-#F57124:100", stroke: "#fff", "stroke-width": 2, r: 0 });

        rx = Math.floor(Math.random() * 1000);
        ry = Math.floor(Math.random() * 400);

        attr = {
            cx: rx,
            cy: ry
        };

        world.Xs.push(attr.cx);
        world.Ys.push(attr.cy);

        dot.stop().attr(attr).animate({ r: 5 }, 1000, "elastic");

        $('#citiesNum').val(parseInt($('#citiesNum').val()) + 1);
    });

    $('#start').click(function () {
        if (world.Xs.length > 2) {
            $.ajax({
                type: "POST",
                url: '/Home/Init',
                traditional: true,
                data: { it: parseInt($('#iterations').val()), perStep: parseInt($('#iterationsPerStep').val()), xs: world.Xs, ys: world.Ys },
                success: function (data) {
                    Iterate();
                },
                error: function (q, w, e) {
                    alert();
                },
                dataType: 'text'
            });
        } else {
            alert('Изберете поне 3 града');
        }
    });
});
