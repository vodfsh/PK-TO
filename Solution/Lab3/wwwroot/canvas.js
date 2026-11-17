(function () {
    const colors = ['#16a34a', '#f59e0b', '#dc2626', '#2563eb'];
    const maxPixels = 800;

    function getCanvas() {
        return document.getElementById('simCanvas');
    }

    function sizeFor(w, h) {
        const scale = maxPixels / Math.max(w, h);
        return {
            scale: scale,
            width: Math.max(1, Math.round(w * scale)),
            height: Math.max(1, Math.round(h * scale))
        };
    }

    window.initCanvas = function (w, h) {
        const canvas = getCanvas();
        if (!canvas) return;

        const size = sizeFor(w, h);
        canvas.width = size.width;
        canvas.height = size.height;
    };

    window.draw = function (frame) {
        const w = frame.width;
        const h = frame.height;
        const scale = frame.scale;

        const size = sizeFor(w, h);
        const canvas = getCanvas();
        if (!canvas) return;

        if (canvas.width !== size.width || canvas.height !== size.height) {
            canvas.width = size.width;
            canvas.height = size.height;
        }

        const ctx = canvas.getContext('2d');
        const width = canvas.width;
        const height = canvas.height;

        ctx.clearRect(0, 0, width, height);
        ctx.fillStyle = '#f8fafc';
        ctx.fillRect(0, 0, width, height);

        // Light 5 m grid.
        const grid = 5;
        ctx.strokeStyle = '#e2e8f0';
        ctx.lineWidth = 1;
        ctx.beginPath();
        for (let x = grid; x < w; x += grid) {
            const px = x * scale;
            ctx.moveTo(px, 0);
            ctx.lineTo(px, height);
        }
        for (let y = grid; y < h; y += grid) {
            const py = height - y * scale;
            ctx.moveTo(0, py);
            ctx.lineTo(width, py);
        }
        ctx.stroke();

        // Room boundary.
        ctx.strokeStyle = '#94a3b8';
        ctx.lineWidth = 2;
        ctx.strokeRect(1, 1, width - 2, height - 2);

        // People: [{x, y, kind}, ...].
        for (const person of frame.people) {
            const px = person.x * scale;
            const py = height - person.y * scale;
            const kind = person.kind | 0;

            ctx.beginPath();
            ctx.arc(px, py, 4, 0, Math.PI * 2);
            ctx.fillStyle = colors[kind] || '#0f172a';
            ctx.fill();
            ctx.strokeStyle = 'rgba(255, 255, 255, 0.8)';
            ctx.lineWidth = 1;
            ctx.stroke();
        }
    };

    window.downloadFile = function (name, text) {
        const blob = new Blob([text], {type: 'application/json'});
        const url = URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        anchor.href = url;
        anchor.download = name;
        document.body.appendChild(anchor);
        anchor.click();
        anchor.remove();
        setTimeout(function () {
            URL.revokeObjectURL(url);
        }, 1000);
    };
})();
