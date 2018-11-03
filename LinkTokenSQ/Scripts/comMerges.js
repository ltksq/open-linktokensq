﻿//jetpl
(function(d,f){"function"===typeof define&&define.amd?define(["jetpl"],f):"object"===typeof exports?module.exports=f(require("jetpl")):d.jetpl=f(d.jetpl)})(this,function(d){var f={},l={error:function(a,c){"object"===typeof console&&console.error("jetpl "+a+"\n"+(c||""));return"jetpl "+a}},g=function(a){a=a||"";/^\#/.test(a)&&(a=document.getElementById(a.substring(1)).innerHTML);this.tpl=a;this.tplinit();f[a]=this};g.prototype.tplCompiler=function(a){var c=0;return'var outStr\x3d"";\n'+a.replace(/(^|%>|}})([\s\S]*?)({{|<%|$)/g,function(a,c,h,k){return c+'outStr+\x3d "'+h.replace(/\\/g,"\\\\").replace(/"/g,'\\"').replace(/\r?\n/g,"\\n")+'";\n'+k}).replace(/(<%=)([\s\S]*?)(%>)/g,"outStr+\x3d ($2);\n").replace(/(<%)(?!=)([\s\S]*?)(%>)/g,"\n$2\n").replace(/{{each\s*([\w."'\][]+)\s*(\w+)\s*(\w+)?}}/g,function(a,e,h,k){a="tps"+c++;return("for(var $p\x3d0; $p\x3c$1.length; $p++){"+(h?"\nvar $2 \x3d $1[$p];\n":"\nvar $item \x3d $1[$p];\n")+(k?"\nvar $3 \x3d $p;\n":"")).replace(/\$1/g,e).replace(/\$2/g,h).replace(/\$3/g,k).replace(/\$p/g,a)}).replace(/{{\/each}}/g,"};\n").replace(/{{if\s+(.*?)}}/g,"if($1){").replace(/{{else ?if (.*?)}}/g,"}else if($1){").replace(/{{else}}/g,"}else{").replace(/{{\/if}}/g,"}").replace(/{{=?([\s\S]*?)}}/g,"outStr+\x3d$1;\n")+"return outStr;"};g.prototype.dataToVars=function(a){a=Object.keys(a||{}).sort();for(var c="";a.length;)var b=a.shift(),c=c+("var "+b+'\x3d dt["'+b+'"];');return c};g.prototype.tplinit=function(){var a=this,c=a.tpl,b=function(b,c){f[b]=f[b]||{};var e=a.dataToVars(c);if(f[b].vars==e)return f[b].refuns;var d=f[b].code||a.tplCompiler(b),g=new Function("dt",e+d);f[b].vars=e;f[b].code=d;return f[b].refuns=g},e=function(a,c){var e=b(a,c);return 1<arguments.length?e(c):function(c){return b(a,c)(c)}};return a.render=function(a,b){if(!a)return l.error("no data");try{var d=e(c,a);if(!b)return e(c)(a);b(d)}catch(f){return b("jetpl "+f),l.error(f,c)}}};d=function(a){return"string"!==typeof a?l.error("Template not found"):new g(a)};d.decimal=function(a,c){c=c?c:2;var b=(Math.round(a*Math.pow(10,c))/Math.pow(10,c)).toString(),e=b.indexOf(".");0>e&&(e=b.length,b+=".");for(;b.length<=e+c;)b+="0";return b};d.decode=function(a){return a.replace(/&amp;/g,"\x26").replace(/&lt;/g,"\x3c").replace(/&gt;/g,"\x3e").replace(/&quot;/g,'"').replace(/&#39;/g,"'")};d.escape=function(a){return String(a||"").replace(/&(?!#?[a-zA-Z0-9]+;)/g,"\x26amp;").replace(/</g,"\x26lt;").replace(/>/g,"\x26gt;").replace(/'/g,"\x26#39;").replace(/"/g,"\x26quot;")};d.toNumber=function(a){return a.toString()};d.date=function(a,c){var b=c||"YYYY-MM-DD",e=new Date(1E3*a),d={"M+":e.getMonth()+1,"D+":e.getDate(),"h+":e.getHours(),"m+":e.getMinutes(),"s+":e.getSeconds(),"q+":Math.floor((e.getMonth()+3)/3),ms:e.getMilliseconds()};/(Y+)/.test(b)&&(b=b.replace(RegExp.$1,(e.getFullYear()+"").substr(4-RegExp.$1.length)));for(var f in d)(new RegExp("("+f+")")).test(b)&&(b=b.replace(RegExp.$1,1==RegExp.$1.length?d[f]:("00"+d[f]).substr((""+d[f]).length)));return b};d.include=function(a,c){var b=document.getElementById(a.replace(/\#/g,"")),b=/input|textarea/i.test(b.nodeName)?b.value:b.innerHTML;return d(b).render(c)};d.version="1.2";return d});

//下拉加载
!function(a){a.fn.dropload=function(b){var c={wrapCell:"",loadDatafun:null,afterDatafun:null},d=a.extend(c,b);return this.each(function(){function f(b){var c=a(b).scrollTop(),e=a(b).height(),f=""==d.wrapCell||null==d.wrapCell?a(document):a(d.wrapCell),g=f.height(),h=c+e>=g;return h}function g(){var e,c=!0;c&&f(b)&&(c=!1,clearTimeout(e),e=setTimeout(function(){(a.isFunction(d.afterDatafun)||null!=d.afterDatafun)&&d.afterDatafun&&d.afterDatafun()},1e3))}var b=a(this);d.page,d.pagesize,a.isFunction(d.loadDatafun)||null!=d.loadDatafun?d.loadDatafun&&d.loadDatafun():d.afterDatafun&&d.afterDatafun(),b.on("scroll",function(){g()})})}}(window.jQuery||window.Zepto||window.$);

//图片延迟加载
!function(a,b){"function"==typeof define&&define.amd?define(["$"],b):"object"==typeof exports?module.exports=b():a.lazyInit=b(window.Zepto||window.jQuery||$)}(this,function(a){function b(){this.config={loadImg:"",container:window,effect:"show",effectArgs:0,placeAttr:"data-src",offset:0,fewPiece:0,event:"scroll",elements:null,load:null}}return a.fn.lazyload=function(c){var d=new b,e=a.extend({elements:a(this)},c);return d.init(e),d},b.prototype={init:function(b){this.config=a.extend(this.config,b),this.elements=a(this.config.elements),this.initImg(),this.bindEvent()},initImg:function(){var d,e,f,g,b=this,c=b.config.fewPiece;if(this.elements.each(function(){var c=a(this);void 0!==c.attr("src")&&c.attr("src")!==!1&&""!=c.attr("src")||!c.is("img")||c.attr("src",b.config.loadImg)}),c>0)for(d=0;c>d;d++)e=a(b.elements),f=b.config.placeAttr,g=e.eq(d).attr(f),e.is("img")?e.eq(d).attr("src",g).removeAttr(f):e.eq(d).css("background-image","url('"+g+"')").removeAttr(f);else"scroll"==this.config.event&&this.load()},bindEvent:function(){var b=a(this.config.container),c=this;b.on(c.config.event,function(){c.load()}),a(window).on("resize",function(){c.load()})},load:function(){var a=this;this.elements.each(function(){this.loaded||(a.checkPosition(this)&&a.show(this),a.config.load&&a.config.load.call(a,this))})},checkPosition:function(b){var f,c=a(b).offset().top,d=window.clientHeight||document.documentElement.clientHeight||document.body.clientHeight;return window.clientWidth||document.documentElement.clientWidth||document.body.clientWidth,f=a(window).scrollTop(),c+this.config.offset<=d+f?!0:!1},show:function(b){var f,c=this,d=a(b),e=b;e.loaded=!1,f=d.attr(c.config.placeAttr),a("<img/>").attr("src",f).on("load",function(){e.loaded=!0,d.hide(),d.is("img")?d.attr("src",f).removeAttr(c.config.placeAttr):d.css("background-image","url('"+f+"')").removeAttr(c.config.placeAttr),d[c.config.effect](c.config.effectArgs)})}},b});

//mBox v2.0 弹层组件移动版
!function(a,b){"function"==typeof define&&define.amd?define(["mBox"],b):"object"==typeof exports?module.exports=b(require("mBox")):a.mBox=b(a.mBox)}(this,function(a){function o(a,b,c,d,e){b&&b.parentNode?b.parentNode.insertBefore(a,b.nextSibling):c&&c.parentNode?c.parentNode.insertBefore(a,c):d&&d.appendChild(a),a.style.display="block"==e?"block":"none",this.backSitu=null}var f,i,j,k,l,m,g,h,n,b=function(a){return document.querySelectorAll(a)},c=document,d={};return b.prototype,d.timer={},d.endfun={},d.extend=function(a,b,c){void 0===c&&(c=!0);for(var d in b)!c&&d in a||(a[d]=b[d]);return a},d.oneven=function(a,b){var c;return/Android|iPhone|SymbianOS|Windows Phone|iPad|iPod/.test(navigator.userAgent)?(a.addEventListener("touchmove",function(){c=!0},!1),a.addEventListener("touchend",function(a){a.preventDefault(),c||b.call(this,a),c=!1},!1),void 0):a.addEventListener("click",function(a){b.call(this,a)},!1)},f={width:"",height:"",boxtype:0,title:[],content:"请稍等,暂无内容！",conStyle:"background-color:#fff;",btnName:[],btnStyle:[],yesfun:null,nofun:null,closefun:null,closeBtn:[!1,1],time:null,fixed:!0,mask:!0,maskClose:!0,maskColor:"rgba(0,0,0,0.5)",padding:"10px 10px",zIndex:1e4,success:null,endfun:null},g=1,h=["jembox"],n=function(a){var b=this,c=JSON.parse(JSON.stringify(f));b.config=d.extend(c,a),i=b.config.content,i&&1===i.nodeType&&(m=document.defaultView.getComputedStyle(i,null).display,j=i.previousSibling,k=i.nextSibling,l=i.parentNode),b.viewInit()},n.prototype={viewInit:function(){var k,l,m,n,o,p,q,r,s,a=this,d=a.config,e=c.createElement("div"),f=d.mask?'<div class="jemboxmask" style="pointer-events:auto;background-color:'+d.maskColor+';"></div>':"",i=function(){var a="object"==typeof d.title,b=void 0!=d.title[1]?d.title[1]:"";return""!=d.title?'<div class="jemboxhead" id="head'+g+'" style="'+b+'">'+(a?d.title[0]:d.title)+"</div>":""}(),j=function(){var c,f,h,i,a=d.btnName,b=a.length,e="width:50%;";return 0!==b&&d.btnName?(1===b&&(f=""!=d.btnStyle?"width:100%;"+d.btnStyle[0].replace(/\s/g,""):"width:100%;",c='<span onytpe="1" style="'+f+'">'+a[0]+"</span>"),2===b&&(h=void 0!=d.btnStyle[0]?e+d.btnStyle[0]:e,i=void 0!=d.btnStyle[1]?e+d.btnStyle[1]:e,c='<span onytpe="0" style="'+i+'">'+a[1]+'</span><span onytpe="1" style="'+h+'">'+a[0]+"</span>"),'<div class="jemboxfoot" id="foot'+g+'">'+c+"</div>"):""}();b("html")[0].style.overflowY="hidden",b("body")[0].style.overflowY="hidden",k=""!=d.width?"width:"+d.width+";":"",l=""!=d.height?"height:"+d.height+";":"",a.id=e.id=h[0]+g,e.setAttribute("class","jemboxer "+h[0]+(d.boxtype||1)),e.setAttribute("style","z-index:"+d.zIndex),e.setAttribute("jmb",g),e.innerHTML=f+'<div class="jemboxmain" '+(d.fixed?"":'style="position:static;"')+'><div class="jemboxsection">'+'<div class="jemboxchild" style="'+k+l+d.conStyle+'">'+i+'<span class="jemboxclose0'+d.closeBtn[1]+'" style="display:'+(d.closeBtn[0]?"block":"none")+'"></span>'+'<div class="jemboxmcont" style="padding:'+d.padding+';"></div>'+j+"</div>"+"</div></div>",c.body.appendChild(e),""!=d.height&&(m=b("#"+a.id+" .jemboxmcont")[0],n=""!=d.title?b("#head"+g)[0].offsetHeight:0,o=0!=d.btnName.length?b("#foot"+g)[0].offsetHeight:0,p=/^\d+%$/.test(d.height.toString())?parseInt(document.documentElement.clientHeight*(d.height.replace("%","")/100)):parseInt(fixH.replace(/\px|em|rem/g,"")),q=m.style.paddingTop.replace(/\px|em|rem/g,""),r=m.style.paddingBottom.replace(/\px|em|rem/g,""),m.style.overflow="auto",m.style.height=p-n-o-q-r+"px"),s=a.elem=b("#"+a.id)[0],setTimeout(function(){try{b("#"+a.id+" .jemboxchild")[0].classList.add("jemboxanim")}catch(c){return}d.success&&d.success(s)},1),a.idx=g++,a.contype(d),a.action(d)},contype:function(a){var g,d=this,e=b("#"+d.id+" .jemboxmcont")[0],f=a.content;return d._elemBack&&d._elemBack(),void 0===f?e:("string"==typeof f?e.innerHTML=a.content:f&&1===f.nodeType&&(g=c.createElement("div"),"none"==window.getComputedStyle(f,null).display&&(f.style.display="block"),e.appendChild(g.appendChild(f))),void 0)},action:function(c){var f,g,h,i,e=this;if(c.time&&(d.timer[e.idx]=setTimeout(function(){a.close(e.idx)},1e3*c.time)),c.closeBtn[0]&&(f=b("#"+e.id+" .jemboxclose0"+c.closeBtn[1])[0],d.oneven(f,function(){c.closefun&&c.closefun(),a.close(e.idx)})),""!=c.btnName)for(g=b("#"+e.id+" .jemboxfoot")[0].children,h=0;h<g.length;h++)d.oneven(g[h],function(){var b=this.getAttribute("onytpe");0==b?c.nofun&&c.nofun():c.yesfun&&c.yesfun(e.idx),a.close(e.idx)});c.mask&&c.maskClose&&(i=b("#"+e.id+" .jemboxmask")[0],d.oneven(i,function(){a.close(e.idx,c.endfun)})),c.endfun&&(d.endfun[e.idx]=c.endfun)}},a={idx:g,version:"2.0",cell:function(a){return b(a)[0]},open:function(a){var b=new n(a||{});return b.idx},close:function(a){var e=b("#jembox"+a)[0];e&&(i&&1===i.nodeType&&o(i,j,k,l,m),c.body.removeChild(e),b("html")[0].style.overflowY="",b("body")[0].style.overflowY="",clearTimeout(d.timer[g]),delete d.timer[g],"function"==typeof d.endfun[g]&&d.endfun[g](),delete d.endfun[g])},closeAll:function(){var d,c=b(".jemboxer");for(d=0;d<c.length;d++)a.close(c[d].getAttribute("jmb"))}}});
//返回到顶部
!function(a){a.fn.goTops=function(b){var c={endY:navigator.userAgent.match(/(Android)/i)?1:0,toBtnCell:"#ontoBtn",direction:"right",showHeight:a(window).height()/2,posLeft:10,posRight:10,posBottom:55,duration:200,zIndex:200,callback:null},d=a.extend(c,b),e=function(a,b,c){return a+(b-a)*c},f=function(a){return-Math.cos(a*Math.PI)/2+.5},g=function(){var a,b,c,g;return 0===d.duration?(window.scrollTo(0,d.endY),"function"==typeof d.callback&&d.callback(),void 0):(a=window.pageYOffset,b=Date.now(),c=b+d.duration,g=function(){var h=Date.now(),i=h>c?1:(h-b)/d.duration;window.scrollTo(0,e(a,d.endY,f(i))),c>h?setTimeout(g,15):"function"==typeof d.callback&&d.callback()},g(),void 0)};return this.each(function(){var f,b=a(this),c=a(d.toBtnCell);navigator.userAgent.match(/(iPhone|iPod|Android|ios|iOS|iPad|Backerry|WebOS|Symbian|Windows Phone|Phone)/i),f="right"==d.direction?{right:d.posRight}:{left:d.posLeft},c.css({"z-index":d.zIndex,position:"fixed",display:"none",bottom:d.posBottom}).css(f),b.on("scroll",function(){var b=a(this).scrollTop();b>=d.showHeight?c.css({display:"block"}):c.css({display:"none"})}),c.on("tap",function(){g()})})}}(window.jQuery||window.Zepto||window.$);