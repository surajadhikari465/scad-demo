/*!@license
* Infragistics.Web.ClientUI infragistics.dv_scroller.js 19.1.20191.376
*
* Copyright (c) 2011-2019 Infragistics Inc.
*
* http://www.infragistics.com/
*
* Depends:
*     jquery-1.4.4.js
*     jquery.ui.core.js
*     jquery.ui.widget.js
*     infragistics.util.js
*     infragistics.ext_core.js
*     infragistics.ext_collections.js
*     infragistics.dv_core.js
*     infragistics.dv_datasource.js
*     infragistics.dv_interactivity.js
*     infragistics.ext_ui.js
*/
(function(factory){if(typeof define==="function"&&define.amd){define(["./infragistics.util","./infragistics.ext_core","./infragistics.ext_collections","./infragistics.dv_core","./infragistics.dv_datasource","./infragistics.dv_interactivity","./infragistics.ext_ui"],factory)}else{factory(igRoot)}})(function($){$.ig=$.ig||{};var $$t={};$.ig.globalDefs=$.ig.globalDefs||{};$.ig.globalDefs.$$bq=$$t;$$0=$.ig.globalDefs.$$0;$$4=$.ig.globalDefs.$$4;$$1=$.ig.globalDefs.$$1;$$w=$.ig.globalDefs.$$w;$$y=$.ig.globalDefs.$$y;$$al=$.ig.globalDefs.$$al;$$6=$.ig.globalDefs.$$6;$$a=$.ig.globalDefs.$$a;$.ig.$currDefinitions=$$t;$.ig.util.bulkDefine(["ScrollerView:a","Scroller:b","ScrollerScrollingEventHandler:c","IScrollerKeyboardListener:e"]);var $a=$.ig.intDivide,$b=$.ig.util.cast,$c=$.ig.util.defType,$d=$.ig.util.defEnum,$e=$.ig.util.getBoxIfEnum,$f=$.ig.util.getDefaultValue,$g=$.ig.util.getEnumValue,$h=$.ig.util.getValue,$i=$.ig.util.intSToU,$j=$.ig.util.nullableEquals,$k=$.ig.util.nullableIsNull,$l=$.ig.util.nullableNotEquals,$m=$.ig.util.toNullable,$n=$.ig.util.toString$1,$o=$.ig.util.u32BitwiseAnd,$p=$.ig.util.u32BitwiseOr,$q=$.ig.util.u32BitwiseXor,$r=$.ig.util.u32LS,$s=$.ig.util.unwrapNullable,$t=$.ig.util.wrapNullable,$u=String.fromCharCode,$v=$.ig.util.castObjTo$t,$w=$.ig.util.compareSimple,$x=$.ig.util.tryParseNumber,$y=$.ig.util.tryParseNumber1,$z=$.ig.util.numberToString,$0=$.ig.util.numberToString1,$1=$.ig.util.parseNumber;$c("ScrollerView:a","Object",{init:function(a){this.d=false;this.c=null;this.h=false;this.g=false;this.t=0;this.q=0;this.u=-1;this.r=-1;$.ig.$op.init.call(this);this._b=a},_b:null,_y:null,_ac:null,az:function(){return function(){var $ret=new $$a.at;$ret.fill("#666666");return $ret}()},_x:null,_a:null,_ab:null,_z:null,_aa:null,_ad:null,as:function(a){var $self=this;if(a==null){if(this._y!=null){this._y=null}if(this._a!=null){this._a._ak=null;this._a.aq()}return}this._y=a;var b=Math.round(this._y.rootWrapper().width());var c=Math.round(this._y.rootWrapper().height());this._ab=this._y.rootWrapper();this._ab.setStyleProperty("position","relative");this._ac=this._y.createElement("div");this._ac.setStyleProperty("position","relative");this._ac.setStyleProperty("width","100%");this._ac.setStyleProperty("height","100%");this._ac.setStyleProperty("overflow","hidden");this._ab.append(this._ac);this._aa=this._y.createElement("div");this._aa.setStyleProperty("position","absolute");this._aa.setStyleProperty("height",this._b.scrollbarSpan()+"px");this._aa.setStyleProperty("width",this._b.minScrollbarSize()+"px");this._aa.setStyleProperty("background-color",this._b.scrollbarBrush().__fill);this._aa.setStyleProperty("display","none");this._aa.setStyleProperty("border-radius","5px");this._aa.setStyleProperty("z-index","10000");this._aa.setStyleProperty("bottom","0px");this._ab.append(this._aa);this._ad=this._y.createElement("div");this._ad.setStyleProperty("position","absolute");this._ad.setStyleProperty("width",this._b.scrollbarSpan()+"px");this._ad.setStyleProperty("height",this._b.minScrollbarSize()+"px");this._ad.setStyleProperty("background-color",this._b.scrollbarBrush().__fill);this._ad.setStyleProperty("display","none");this._ad.setStyleProperty("border-radius","5px");this._ad.setStyleProperty("z-index","10000");this._ad.setStyleProperty("right","0px");this._ab.append(this._ad);this._x=this._ac.getNativeElement();this._a=new $.ig.CanvasGestureDOMEventProxy(this._y.rootWrapper(),this._y,true);this._y.rootWrapper().setProperty("tabIndex",1e3);this._y.rootWrapper().setRawStyleProperty("outline","none");this._a._c1=true;var d=this._a;d.onMouseWheel=$.ig.Delegate.prototype.combine(d.onMouseWheel,this.k.runOn(this));var e=this._a;e.onMouseWheelHorizontal=$.ig.Delegate.prototype.combine(e.onMouseWheelHorizontal,this.l.runOn(this));var f=this._a;f.onDragStarted=$.ig.Delegate.prototype.combine(f.onDragStarted,this.ak.runOn(this));var g=this._a;g.onDragDelta=$.ig.Delegate.prototype.combine(g.onDragDelta,this.aj.runOn(this));var h=this._a;h.onDragCompleted=$.ig.Delegate.prototype.combine(h.onDragCompleted,this.ai.runOn(this));var i=this._a;i.onFlingStarted=$.ig.Delegate.prototype.combine(i.onFlingStarted,this.i.runOn(this));var j=this._a;j.onContactStarted=$.ig.Delegate.prototype.combine(j.onContactStarted,this.ah.runOn(this));var k=this._a;k.onContactMoved=$.ig.Delegate.prototype.combine(k.onContactMoved,this.ag.runOn(this));var l=this._a;l.onContactCompleted=$.ig.Delegate.prototype.combine(l.onContactCompleted,this.af.runOn(this));this._a._aa=true;this._a._am=this.o.runOn(this);var m=this._a;m.onKeyDown=$.ig.Delegate.prototype.combine(m.onKeyDown,this.j.runOn(this));this._a._an=function(n){var e_=n.originalEvent;var o=e_.type;if(o=="pointerdown"&&!$self._a.bl(n)){return false}return true};this._b.b4(b,c)},j:function(a){return this._b.q(a,(this._a._bc&4)!=0,(this._a._bc&2)!=0)},o:function(a,b,c){if(this.e||this.f){return true}if(c||b){return true}if(this.m(a,b)){return true}if(this.n(a,b)){return true}return false},af:function(a,b){this.e=false;this.f=false},ag:function(a,b){if(this.f){var c=a.__y-this.s;this.s=a.__y;this._b.b3(c)}else if(this.e){var d=a.__x-this.p;this.p=a.__x;this._b.b1(d)}},ah:function(a,b){this.d=false;this.ae();var c=this.n(a,b);var d=this.m(a,b);if(c){this.f=true;this.s=a.__y;this.al()}else if(d){this.e=true;this.p=a.__x;this.al()}},m:function(a,b){if(!this.g){return false}if(a.__y>=this._b.viewportHeight()-this._b.scrollbarSpan()&&a.__y<=this._b.viewportHeight()&&a.__x>=this.r-this.q/2&&a.__x<=this.r+this.q/2){return true}return false},n:function(a,b){if(!this.h){return false}if(a.__x>=this._b.viewportWidth()-this._b.scrollbarSpan()&&a.__x<=this._b.viewportWidth()&&a.__y>=this.u-this.t/2&&a.__y<=this.u+this.t/2){return true}return false},d:false,i:function(a,b,c){this.d=true;return true},aq:function(a){this._a._bf=new $$a.ae(0,0,0,this._b.viewportWidth(),this._b.viewportHeight())},ar:function(a){this._a._bf=new $$a.ae(0,0,0,this._b.viewportWidth(),this._b.viewportHeight())},ao:function(a){if(this._z!=null){this._z.setStyleProperty("height",a+"px")}},ap:function(a){if(this._z!=null){this._z.setStyleProperty("width",a+"px")}},ae:function(){},ai:function(a){if(this.e||this.f){return}this.d=false;this._b.by(a)},aj:function(a){if(this.e||this.f){return}this._b.bz(a)},ax:function(a){this._x.scrollTop=$.ig.truncate(a)},ak:function(a){if(this.e||this.f){return}this.d=false;this._b.b0(a)},v:function(){return this._x.scrollLeft},w:function(){return this._x.scrollTop},k:function(a,b){this.d=false;this.ae();return this._b.s(a,0,b)},l:function(a,b){this.d=false;this.ae();return this._b.s(a,b,0)},aw:function(a){this._x.scrollLeft=$.ig.truncate(a)},at:function(a){this._z=a;this._ac.append(this._z)},c:null,au:function(a){if(this._y==null){return}if(this.c==null){this.c=this._y.getRequestAnimationFrame()}if(this.d){a()}else{this.c(function(){a()})}},h:false,g:false,t:0,q:0,u:0,r:0,f:false,e:false,s:0,p:0,an:function(){if(this.h){this.h=false;this._ad.setStyleProperty("display","none")}},am:function(){if(this.g){this.g=false;this._aa.setStyleProperty("display","none")}},ay:function(a,b){var c=false;if(!this.h){c=true;this.h=true;this._ad.setStyleProperty("display","block")}if(a!=this.u||c){this.u=a;if(this.h){this._ad.setRawYPosition(a-b/2)}}if(b!=this.t||c){this.t=b;if(this.h){this._ad.setRawSize(this._b.scrollbarSpan(),b)}}},av:function(a,b){var c=false;if(!this.g){c=true;this.g=true;this._aa.setStyleProperty("display","block")}if(a!=this.r||c){this.r=a;if(this.g){this._aa.setRawXPosition(a-b/2)}}if(b!=this.q||c){this.q=b;if(this.g){this._aa.setRawSize(b,this._b.scrollbarSpan())}}},al:function(){this._y.rootWrapper().focus()},$type:new $.ig.Type("ScrollerView",$.ig.$ot)},true);$c("Scroller:b","Object",{init:function(){this.aq=49;this.ai=NaN;this.ad=49;this.ab=NaN;this.ap=48;this.ah=NaN;this.ac=48;this.aa=NaN;this.ag=0;this.aj=20;this.am=8;this.ar=0;this.cf=null;this.k=false;this.af=0;this.at=0;this.as=0;this.ao=0;this.an=0;this.c=null;this.i=true;this.ae=0;this.z=0;this.h=false;this.f=false;this.g=false;this.e=false;this.ak=0;this.al=0;this.j=false;$.ig.$op.init.call(this);this.cf=this.view().az()},aq:0,smallVerticalChange:function(a){if(arguments.length===1){var b=this.aq;this.aq=a;if(b!=this.aq){this.b2("SmallVerticalChange",b,this.aq)}return a}else{return this.aq}},_keyboardListener:null,keyboardListener:function(a){if(arguments.length===1){this._keyboardListener=a;return a}else{return this._keyboardListener}},ai:0,largeVerticalChange:function(a){if(arguments.length===1){var b=this.ai;this.ai=a;if(b!=this.ai){this.b2("LargeVerticalChange",b,this.ai)}return a}else{return this.ai}},ad:0,ay:function(a){if(arguments.length===1){var b=this.ad;this.ad=a;if(b!=this.ad){this.b2("ActualSmallVerticalChange",b,this.ad)}return a}else{return this.ad}},ab:0,aw:function(a){if(arguments.length===1){var b=this.ab;this.ab=a;if(b!=this.ab){this.b2("ActualLargeVerticalChange",b,this.ab)}return a}else{return this.ab}},ap:0,smallHorizontalChange:function(a){if(arguments.length===1){var b=this.ap;this.ap=a;if(b!=this.ap){this.b2("SmallHorizontalChange",b,this.ap)}return a}else{return this.ap}},ah:0,largeHorizontalChange:function(a){if(arguments.length===1){var b=this.ah;this.ah=a;if(b!=this.ah){this.b2("LargeHorizontalChange",b,this.ah)}return a}else{return this.ah}},ac:0,ax:function(a){if(arguments.length===1){var b=this.ac;this.ac=a;if(b!=this.ac){this.b2("ActualSmallHorizontalChange",b,this.ac)}return a}else{return this.ac}},aa:0,av:function(a){if(arguments.length===1){var b=this.aa;this.aa=a;if(b!=this.aa){this.b2("ActualLargeHorizontalChange",b,this.aa)}return a}else{return this.aa}},ag:0,contentWidth:function(a){if(arguments.length===1){var b=this.ag;this.ag=a;if(b!=this.ag){this.b2("ContentWidth",b,this.ag)}return a}else{return this.ag}},aj:0,minScrollbarSize:function(a){if(arguments.length===1){var b=this.aj;this.aj=a;if(b!=this.aj){this.b2("MinScrollbarSize",b,this.aj)}return a}else{return this.aj}},am:0,scrollbarSpan:function(a){if(arguments.length===1){var b=this.am;this.am=a;if(b!=this.am){this.b2("ScrollbarSpan",b,this.am)}return a}else{return this.am}},q:function(a,b,c){switch(a){case 11:return this.t(b,c);case 10:return this.u(b,c);case 17:return this.n(b,c);case 15:return this.x(b,c);case 14:return this.r(b,c);case 16:return this.v(b,c);case 13:return this.p(b,c);case 12:return this.o(b,c);case 2:return this.w(b,c)}return false},w:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onTab(a,b)){return true}}return false},o:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onEnd(a,b)){return true}}return false},p:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onHome(a,b)){return true}}return false},v:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onRightArrow(a,b)){return true}}if(this.scrollLeft()+this.viewportWidth()>=this.contentWidth()){return false}this.b9(this.ax(),0);return true},r:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onLeftArrow(a,b)){return true}}if(this.scrollLeft()<=0){return false}this.b9(this.ax()*-1,0);return true},x:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onUpArrow(a,b)){return true}}if(this.scrollTop()<=0){return false}this.b9(0,this.ay()*-1);return true},n:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onDownArrow(a,b)){return true}}if(this.scrollTop()+this.viewportHeight()>=this.contentHeight()){return false}this.b9(0,this.ay());return true},u:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onPageUp(a,b)){return true}}if(this.scrollTop()<0){return false}this.b9(0,this.aw()*-1);return true},t:function(a,b){if(this.keyboardListener()!=null){if(this.keyboardListener().onPageDown(a,b)){return true}}if(this.scrollTop()+this.viewportHeight()>=this.contentHeight()){return false}this.b9(0,this.aw());return true},ar:0,verticalTrackStartInset:function(a){if(arguments.length===1){var b=this.ar;this.ar=a;if(b!=this.ar){this.b2("VerticalTrackStartInset",b,this.ar)}return a}else{return this.ar}},cf:null,scrollbarBrush:function(a){if(arguments.length===1){var b=this.cf;this.cf=a;if(b!=this.cf){this.b2("ScrollbarBrush",b,this.cf)}return a}else{return this.cf}},b2:function(a,b,c){if(this.propertyChanged!=null){this.propertyChanged(this,new $$0.b7(a))}this.b5(a,b,c)},k:false,y:function(a){if(arguments.length===1){this.k=a;if(!this.k){this.view().ax(this.scrollTop());this.view().aw(this.scrollLeft())}return a}else{return this.k}},b5:function(a,b,c){switch(a){case"ContentWidth":this.view().ap(this.contentWidth());this.cc();break;case"ContentHeight":this.view().ao(this.contentHeight());this.ce();break;case"ScrollTop":if(!this.y()){this.view().ax(this.scrollTop())}break;case"ScrollLeft":if(!this.y()){this.view().aw(this.scrollLeft())}break;case"ViewportWidth":this.view().ar(this.viewportWidth());if($.ig.util.isNaN(this.largeVerticalChange())){this.av(this.viewportWidth())}break;case"ViewportHeight":this.view().aq(this.viewportHeight());if($.ig.util.isNaN(this.largeVerticalChange())){this.aw(this.viewportHeight())}break;case"ScrollbarBrush":case"ScrollbarSpan":case"MinScrollbarSize":case"VerticalTrackStartInset":this.ce();this.cc();break}},af:0,contentHeight:function(a){if(arguments.length===1){var b=this.af;this.af=a;if(b!=this.af){this.b2("ContentHeight",b,this.af)}return a}else{return this.af}},b3:function(a){var b=Math.max(this.minScrollbarSize(),this.actualVerticalScrollbarHeight());var c=this.verticalTrackStartInset()+b/2;var d=this.viewportHeight()-b/2;var e=a/(d-c);var f=e*(this.contentHeight()-this.viewportHeight());this.b9(0,f)},b1:function(a){var b=Math.max(this.minScrollbarSize(),this.actualHorizontalScrollbarWidth());var c=0+b/2;var d=this.viewportWidth()-b/2;var e=a/(d-c);var f=e*(this.contentWidth()-this.viewportWidth());this.b9(f,0)},b4:function(a,b){this.at=a;this.as=b},at:0,viewportWidth:function(a){if(arguments.length===1){var b=this.at;this.at=a;if(b!=this.at){this.b2("ViewportWidth",b,this.at)}return a}else{return this.at}},as:0,viewportHeight:function(a){if(arguments.length===1){var b=this.as;this.as=a;if(b!=this.as){this.b2("ViewportHeight",b,this.as)}return a}else{return this.as}},ao:0,scrollTop:function(a){if(arguments.length===1){var b=this.ao;this.ao=a;if(b!=this.ao){this.b2("ScrollTop",b,this.ao)}return a}else{return this.ao}},an:0,scrollLeft:function(a){if(arguments.length===1){var b=this.an;this.an=a;if(b!=this.an){this.b2("ScrollLeft",b,this.an)}return a}else{return this.an}},c:null,view:function(){if(this.c==null){this.c=new $$t.a(this)}return this.c},provideContainer:function(a){this.view().as(a);this.ao=this.view().w();this.an=this.view().v();this.cc();this.ce()},provideContent:function(a){this.view().at(a)},s:function(a,b,c){this.g=false;this.f=false;this.h=false;this.view().ae();if(this.keyboardListener()!=null){this.keyboardListener().onWheel()}this.b9(b,c);return true},i:false,lockScrollDirection:function(a){if(arguments.length===1){this.i=true;return a}else{return this.i}},_l:false,ae:0,actualVerticalScrollbarHeight:function(a){if(arguments.length===1){var b=this.ae;this.ae=a;if(b!=this.ae){this.b2("ActualVerticalScrollbarHeight",b,this.ae)}return a}else{return this.ae}},z:0,actualHorizontalScrollbarWidth:function(a){if(arguments.length===1){var b=this.z;this.z=a;if(b!=this.z){this.b2("ActualHorizontalScrollbarWidth",b,this.z)}return a}else{return this.z}},h:false,f:false,g:false,ch:null,e:false,propertyChanged:null,focus:function(){this.view().al()},b0:function(a){this.view().al();this.h=false;this.f=false;this.g=false;this.e=true;this.ch={__x:a.__x,__y:a.__y,$type:$$a.$y.$type,getType:$.ig.$op.getType,getGetHashCode:$.ig.$op.getGetHashCode,typeName:$.ig.$op.typeName}},bz:function(a){if(this.e){var b=a.__y-this.ch.__y;var c=a.__x-this.ch.__x;if(this.lockScrollDirection()&&!this.g){this.g=true;if(b>0||c>0){if(b>c){this.h=true}else{this.h=true}}}this.ch={__x:a.__x,__y:a.__y,$type:$$a.$y.$type,getType:$.ig.$op.getType,getGetHashCode:$.ig.$op.getGetHashCode,typeName:$.ig.$op.typeName};this.b9(-c,-b)}},onScrolling:null,scrollTo:function(a,b){this.ak=a-this.scrollLeft();this.al=b-this.scrollTop();this.b8()},ak:0,al:0,b9:function(a,b){this.ak+=a;this.al+=b;this.b8()},j:false,b8:function(){if(this.j){return}this.j=true;this.view().au(this.cb.runOn(this))},cb:function(){this.j=false;var a=this.ak;var b=this.al;this.ak=0;this.al=0;if(this.g){if(this.h){a=0}if(this.f){b=0}}var c=Math.round(this.scrollTop()+b);var d=Math.round(this.scrollLeft()+a);if(c<0){c=0;this.view().ae()}if(d<0){d=0;this.view().ae()}if(c+this.viewportHeight()>this.contentHeight()){c=this.contentHeight()-this.viewportHeight();if(c<0){c=0}this.view().ae()}if(d+this.viewportWidth()>this.contentWidth()){d=this.contentWidth()-this.viewportWidth();if(d<0){d=0}this.view().ae()}b=c-this.scrollTop();a=d-this.scrollLeft();if(b!=0||a!=0){try{this.y(true);this._l=true;this.scrollTop(c);this.scrollLeft(d);this.ce();this.cc();this._l=false;if(this.onScrolling!=null){this.onScrolling(this,function(){var $ret=new $$t.d;$ret.deltaX(a);$ret.deltaY(b);return $ret}())}}finally{this.y(false)}}},ce:function(){this.cd(this.contentHeight(),this.viewportHeight(),this.scrollTop(),true)},cc:function(){this.cd(this.contentWidth(),this.viewportWidth(),this.scrollLeft(),false)},cd:function(a,b,c,d){var e=Math.round(b/a*b);var f=d?this.verticalTrackStartInset():0;e=e-f;if(e<this.minScrollbarSize()){e=this.minScrollbarSize()}if(e>=b){if(d){this.view().an()}else{this.view().am()}return}var g=f+e/2;var h=b-e/2;var i=c/(a-b);var j=Math.round(g+(h-g)*i);if(d){this.actualVerticalScrollbarHeight(e);this.view().ay(j,e)}else{this.actualHorizontalScrollbarWidth(e);this.view().av(j,e)}},by:function(a){this.g=false;this.f=false;this.h=false;this.e=false},$type:new $.ig.Type("Scroller",$.ig.$ot,[$$0.$b6.$type])},true);$c("ScrollerScrollingEventArgs:d","EventArgs",{init:function(){$$0.$w.init.call(this)},_deltaX:0,deltaX:function(a){if(arguments.length===1){this._deltaX=a;return a}else{return this._deltaX}},_deltaY:0,deltaY:function(a){if(arguments.length===1){this._deltaY=a;return a}else{return this._deltaY}},$type:new $.ig.Type("ScrollerScrollingEventArgs",$$0.$w.$type)},true);$c("IScrollerKeyboardListener:e","Object",{$type:new $.ig.Type("IScrollerKeyboardListener",null)},true)});(function(factory){if(typeof define==="function"&&define.amd){define("watermark",["jquery"],factory)}else{factory(jQuery)}})(function($){$(document).ready(function(){var wm=$("#__ig_wm__").length>0?$("#__ig_wm__"):$("<div id='__ig_wm__'></div>").appendTo(document.body);wm.css({position:"fixed",bottom:0,right:0,zIndex:1e3}).addClass("ui-igtrialwatermark")})});