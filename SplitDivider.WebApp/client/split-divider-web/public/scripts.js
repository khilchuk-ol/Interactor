var points = [],
  velocity2 = 5, // velocity squared
  radius = 5,
  boundaryX = 600,
  boundaryY = 600,
  numberOfPoints = 100;

init();

function init() {
  // create points
  for (let i = 0; i < numberOfPoints; i++) {
    createPoint();
  }
  // create connections
  for (let i = 0, l = points.length; i < l; i++) {
    if (i === 0) {
      points[i].buddy = points[points.length - 1];
    } else {
      points[i].buddy = points[i - 1];
    }
  }
}

function createPoint() {
  var point = {},
    vx2,
    vy2;

  point.x = Math.random() * boundaryX;
  point.y = Math.random() * boundaryY;
  // random vx
  point.vx = (Math.floor(Math.random()) * 2 - 1) * Math.random();
  vx2 = Math.pow(point.vx, 2);
  // vy^2 = velocity^2 - vx^2
  vy2 = velocity2 - vx2;
  point.vy = Math.sqrt(vy2) * (Math.random() * 2 - 1);
  points.push(point);
}

function resetVelocity(point, axis, dir) {
  if (axis === "x") {
    point.vx = dir * Math.random();

    let vx2 = Math.pow(point.vx, 2);
    // vy^2 = velocity^2 - vx^2
    let vy2 = velocity2 - vx2;
    point.vy = Math.sqrt(vy2) * (Math.random() * 2 - 1);
  } else {
    point.vy = dir * Math.random();
    let vy2 = Math.pow(point.vy, 2);
    // vy^2 = velocity^2 - vx^2
    let vx2 = velocity2 - vy2;
    point.vx = Math.sqrt(vx2) * (Math.random() * 2 - 1);
  }
}

function drawCircle(context, x, y) {
  context.beginPath();
  context.arc(x, y, radius, 0, 2 * Math.PI, false);
  context.fillStyle = "#2d2d2d";
  context.fill();
}

function drawLine(context, x1, y1, x2, y2) {
  context.beginPath();
  context.moveTo(x1, y1);
  context.lineTo(x2, y2);
  context.strokeStyle = "#424242";
  context.stroke();
}

function draw(context) {
  for (var i = 0, l = points.length; i < l; i++) {
    // circles
    var point = points[i];
    point.x += point.vx;
    point.y += point.vy;
    drawCircle(context, point.x, point.y);
    // lines
    drawLine(context, point.x, point.y, point.buddy.x, point.buddy.y);
    // check for edge
    if (point.x < radius) {
      resetVelocity(point, "x", 1);
    } else if (point.x > boundaryX - radius) {
      resetVelocity(point, "x", -1);
    } else if (point.y < radius) {
      resetVelocity(point, "y", 1);
    } else if (point.y > boundaryY - radius) {
      resetVelocity(point, "y", -1);
    }
  }
}

function animate() {
  let canvas = document.getElementById("graphic");
  let context = canvas.getContext("2d");

  context.clearRect(0, 0, 600, 600);

  draw(context);
  requestAnimationFrame(animate);
}
