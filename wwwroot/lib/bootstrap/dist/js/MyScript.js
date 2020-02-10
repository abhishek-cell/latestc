jQuery(document).ready(function ($) {
    $('#image1').addimagezoom({
        zoomrange: [3, 10],
        magnifiersize: [300, 300],
        magnifierpos: 'right',
        cursorshade: true,
        largeimage: 'Image/image1.jpg' //<-- No comma after last option!
    })
    $('#image2').addimagezoom({
        zoomrange: [5, 5],
        magnifiersize: [400, 400],
        magnifierpos: 'right',
        cursorshade: true,
        cursorshadecolor: 'pink',
        cursorshadeopacity: 0.3,
        cursorshadeborder: '1px solid red',
        largeimage: 'Image/image2.jpg' //<-- No comma after last option!
    })
    $('#image3').addimagezoom()
})
