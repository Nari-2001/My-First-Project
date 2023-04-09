// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//api url is like https://forkify-api.herokuapp.com/api/v2/recipes?search=pizza&key=<insert your key>

let apiUrl = "https://forkify-api.herokuapp.com/api/v2/recipes";
let apiKey = "12adac2e-368b-475f-98a2-6e552118390f";   //generate our API key value(in forkify website we have a option 'Generate you API key' click that then it gives one generated api key value use that value here)

async function GetRecipes(recipeName,id,isAllShow) {
    let resp = await fetch(`${apiUrl}?search=${recipeName}&key=${apiKey}`);
    let result = await resp.json();
    //console.log(result);
    let items = isAllShow ? result.data.recipes : result.data.recipes.slice(0,6);
    showRecipes(items, id);
}
function showRecipes(recipeitems, id) {
    $.ajax({
        contentType: "application/json;charset=utf-8",
        dataType: 'html',
        type: 'post',
        url: '/Recipe/GetRecipeCard',
        data: JSON.stringify(recipeitems),
        success: function (htmlContent) {
            $('#' + id).html(htmlContent);
            getCartItems    ();
        }
    });
}


//https://forkify-api.herokuapp.com/api/v2/recipes/5ed6604591c37cdc054bc886?key=<insert your key>
async function getOrders(id,showId) {
    let resp = await fetch(`${apiUrl}/${id}?key=${apiKey}`);
    let resu = await resp.json();
    //console.log(resu);
    showOrders(resu.data.recipe, showId)
}
function showOrders(orderdetails,showId) {
    $.ajax({
        url: "/Recipe/ShowOrderDetails",
        type: 'post',
        dataType: 'html',
        data: orderdetails,
        success: function(data) {
            $('#' + showId).html(data)
        }
    });
}

function onIncDec(symbol) {
    var qty=$('#qtytextbox').val();
    var prize =parseInt($('#price').val());
    var totalamount = 0;
    if (symbol == 'inc') {
        qty =parseInt(qty)+1;          /* //increment one,textbox value comes as a string so 1st time concatinate so thats why we convert to 'int' then add +1*/
    } else {
        if (qty == 1) {
            qty = 1;
        } else {
            qty =qty-1;
        }
    }
    totalamount = prize * qty;
    $('#qtytextbox').val(qty);
    $("#totalAmount").val(totalamount);
}


//Add to cart
async function cart() {
    //console.log($(this));
    var itag = $(this).children('i')[0];
    //console.log(itag);
    var recpid = $(this).attr('recipeofid');
    //console.log(recpid)
    //console.log(itag);
    //console.log(recpid);
    if ($(itag).hasClass('fa-regular')) {
        var apiurl = 'https://forkify-api.herokuapp.com/api/v2/recipes';
        var apikey = '12adac2e-368b-475f-98a2-6e552118390f';
        //https://forkify-api.herokuapp.com/api/v2/recipes/5ed6604591c37cdc054bc886?key=<insert your key>
        //url we used already in 'site.js' file
        let resp = await fetch(`${apiurl}/${recpid}?key=${apikey}`);
        let result = await resp.json();
        var cartitem = result.data.recipe;
        //console.log(cartitem);
        cartitem.Recipeid = recpid;   //add 'recipeid' for define one
        delete cartitem.id;   //delete 'id' name existing,because in our model class 'id' property we taken 'RecipeId',so thats why
        //console.log(cartitem);
        cartRequest(cartitem, 'SaveCart', 'fa-solid', 'fa-regular', itag,false);
    } else {
       //now to send action method we need one value that is recipe id,we get recipe id of clicled heart from 'recpid' varible,above we already write
        //console.log(data);
        let data = { Id: recpid }
        cartRequest(data, 'RemoveCartItem', 'fa-regular', 'fa-solid', itag,false);

    }
}

function cartRequest(cartrecipedata,actionmethod, addchild, removechild, parent,isReload) {
    //console.log(cartrecipedata);
    //console.log(parent);
    $.ajax({
        url: '/Cart/'+actionmethod,
        type: 'post',
        data: cartrecipedata,
        success: function (resp) {
            //console.log(resp);
            if (isReload) {
                location.reload();    //reload the current page,because when user click 'remove',then internally goes to action method from that remove cart item in database,that if we want to show then we need to do reload
            }
            else {
                $(parent).addClass(addchild);
                $(parent).removeClass(removechild);
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getCartItems() {
    $.ajax({
        url: '/Cart/GetCartItems',
        type: 'get',
        datatype: 'json',
        success: function (result) {
            $(".Addtocarticon").each((index, spanTag) => {
                let recipeid = $(spanTag).attr("recipeofid");
                //console.log(recipeid);
                for (var i = 0; i <= result.length; i++) {
                    if (recipeid == result[i]) {
                        let itag = $(spanTag).children('i')[0];
                        $(itag).addClass('fa-solid');
                        $(itag).removeClass('fa-regular');
                        break;
                    }
                }

            })
            //console.log(result);

        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getCartListItemsFromDB() {
    $.ajax({
        url: '/Cart/GetCartDetails',
        type: 'get',
        dataType: 'html',
        success: function (result) {
            //console.log(result);
            $('#cartitemsshow').html(result);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function removeCartfromList(id) {
    console.log(id);
    let data = { Id: id };
    cartRequest(data,'RemoveCartItem',null,null,null,true)
}
    