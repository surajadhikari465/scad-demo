/*!@license
* Infragistics.Web.ClientUI infragistics.dv_visualdata.js 19.1.20191.376
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
*     infragistics.ext_ui.js
*/
(function(factory){if(typeof define==="function"&&define.amd){define(["./infragistics.util","./infragistics.ext_core","./infragistics.ext_collections","./infragistics.dv_core","./infragistics.ext_ui"],factory)}else{factory(igRoot)}})(function($){$.ig=$.ig||{};var $$t={};$.ig.globalDefs=$.ig.globalDefs||{};$.ig.globalDefs.$$ap=$$t;$$0=$.ig.globalDefs.$$0;$$4=$.ig.globalDefs.$$4;$$1=$.ig.globalDefs.$$1;$$w=$.ig.globalDefs.$$w;$$6=$.ig.globalDefs.$$6;$$a=$.ig.globalDefs.$$a;$.ig.$currDefinitions=$$t;$.ig.util.bulkDefine(["IVisualData:a"]);var $a=$.ig.intDivide,$b=$.ig.util.cast,$c=$.ig.util.defType,$d=$.ig.util.defEnum,$e=$.ig.util.getBoxIfEnum,$f=$.ig.util.getDefaultValue,$g=$.ig.util.getEnumValue,$h=$.ig.util.getValue,$i=$.ig.util.intSToU,$j=$.ig.util.nullableEquals,$k=$.ig.util.nullableIsNull,$l=$.ig.util.nullableNotEquals,$m=$.ig.util.toNullable,$n=$.ig.util.toString$1,$o=$.ig.util.u32BitwiseAnd,$p=$.ig.util.u32BitwiseOr,$q=$.ig.util.u32BitwiseXor,$r=$.ig.util.u32LS,$s=$.ig.util.unwrapNullable,$t=$.ig.util.wrapNullable,$u=String.fromCharCode,$v=$.ig.util.castObjTo$t,$w=$.ig.util.compareSimple,$x=$.ig.util.tryParseNumber,$y=$.ig.util.tryParseNumber1,$z=$.ig.util.numberToString,$0=$.ig.util.numberToString1,$1=$.ig.util.parseNumber,$2=$.ig.util.compare,$3=$.ig.util.replace,$4=$.ig.util.stringFormat,$5=$.ig.util.stringFormat1,$6=$.ig.util.stringFormat2,$7=$.ig.util.stringCompare1,$8=$.ig.util.stringCompare2,$9=$.ig.util.stringCompare3;$c("IVisualData:a","Object",{$type:new $.ig.Type("IVisualData",null)},true);$c("PrimitiveVisualData:p","Object",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$.ig.$op.init.call(this)},init1:function(a,b){$.ig.$op.init.call(this);this.name(b);this.tags(new $$t.r);this.appearance(new $$t.n)},_appearance:null,appearance:function(a){if(arguments.length===1){this._appearance=a;return a}else{return this._appearance}},_tags:null,tags:function(a){if(arguments.length===1){this._tags=a;return a}else{return this._tags}},type:function(){},_name:null,name:function(a){if(arguments.length===1){this._name=a;return a}else{return this._name}},scaleByViewport:function(a){this.appearance().m(a)},getPoints:function(a){var b=new $$4.x($$4.$x.$type.specialize($$t.$i.$type),0);this.getPointsOverride(b,a);return b},getPointsOverride:function(a,b){},serialize:function(){var a=new $$6.aj(0);a.u("{");a.u("appearance: "+(this.appearance()!=null?this.appearance().serialize():"null")+", ");a.u("tags: [");for(var b=0;b<this.tags().count();b++){if(b!=0){a.l(", ")}a.l('"'+this.tags().__inner[b]+'"')}a.u("],");a.u('type: "'+this.type()+'", ');a.u('name: "'+this.name()+'", ');a.u(this.e());a.u("}");return a.toString()},e:function(){return""},$type:new $.ig.Type("PrimitiveVisualData",$.ig.$ot,[$$t.$a.$type])},true);$c("PrimitiveVisualDataList:b","List$1",{init:function(){$$4.$x.init.call(this,$$t.$p.$type,0)},containingTag:function(a){var b=new $$t.b;for(var c=0;c<this.count();c++){var d=this.__inner[c];for(var e=0;e<d.tags().count();e++){if(d.tags().__inner[e]==a){b.add(d);break}}}return b},serialize:function(){var a=new $$6.aj(0);a.l("{ items: [");for(var b=0;b<this.count();b++){if(b!=0){a.l(", ")}a.l(this.__inner[b].serialize())}return a.toString()},$type:new $.ig.Type("PrimitiveVisualDataList",$$4.$x.$type.specialize($$t.$p.$type),[$$t.$a.$type])},true);$c("LabelAppearanceData:c","Object",{init:function(){$.ig.$op.init.call(this)},_text:null,text:function(a){if(arguments.length===1){this._text=a;return a}else{return this._text}},_horizontalAlignment:null,horizontalAlignment:function(a){if(arguments.length===1){this._horizontalAlignment=a;return a}else{return this._horizontalAlignment}},_verticalAlignment:null,verticalAlignment:function(a){if(arguments.length===1){this._verticalAlignment=a;return a}else{return this._verticalAlignment}},_textAlignment:null,textAlignment:function(a){if(arguments.length===1){this._textAlignment=a;return a}else{return this._textAlignment}},_textWrapping:null,textWrapping:function(a){if(arguments.length===1){this._textWrapping=a;return a}else{return this._textWrapping}},_textPosition:null,textPosition:function(a){if(arguments.length===1){this._textPosition=a;return a}else{return this._textPosition}},_labelBrush:null,labelBrush:function(a){if(arguments.length===1){this._labelBrush=a;return a}else{return this._labelBrush}},_labelBrushExtended:null,labelBrushExtended:function(a){if(arguments.length===1){this._labelBrushExtended=a;return a}else{return this._labelBrushExtended}},_angle:0,angle:function(a){if(arguments.length===1){this._angle=a;return a}else{return this._angle}},_opacity:0,opacity:function(a){if(arguments.length===1){this._opacity=a;return a}else{return this._opacity}},_visibility:false,visibility:function(a){if(arguments.length===1){this._visibility=a;return a}else{return this._visibility}},_font:null,font:function(a){if(arguments.length===1){this._font=a;return a}else{return this._font}},_fontFamily:null,fontFamily:function(a){if(arguments.length===1){this._fontFamily=a;return a}else{return this._fontFamily}},_fontSize:0,fontSize:function(a){if(arguments.length===1){this._fontSize=a;return a}else{return this._fontSize}},_fontWeight:null,fontWeight:function(a){if(arguments.length===1){this._fontWeight=a;return a}else{return this._fontWeight}},_fontStyle:null,fontStyle:function(a){if(arguments.length===1){this._fontStyle=a;return a}else{return this._fontStyle}},_fontStretch:null,fontStretch:function(a){if(arguments.length===1){this._fontStretch=a;return a}else{return this._fontStretch}},_marginLeft:0,marginLeft:function(a){if(arguments.length===1){this._marginLeft=a;return a}else{return this._marginLeft}},_marginRight:0,marginRight:function(a){if(arguments.length===1){this._marginRight=a;return a}else{return this._marginRight}},_marginTop:0,marginTop:function(a){if(arguments.length===1){this._marginTop=a;return a}else{return this._marginTop}},_marginBottom:0,marginBottom:function(a){if(arguments.length===1){this._marginBottom=a;return a}else{return this._marginBottom}},serialize:function(){var a=new $$6.aj(0);a.u("{");a.u('text: "'+(this.text()!=null?this.text():"")+'", ');if(this.textAlignment()!=null){a.u('textAlignment: "'+this.textAlignment()+'", ')}if(this.textWrapping()!=null){a.u('textWrapping: "'+this.textWrapping()+'", ')}a.u("labelBrush: "+(this.labelBrush()!=null?this.labelBrush().serialize():"null")+", ");a.u("labelBrushExtended: "+(this.labelBrushExtended()!=null?this.labelBrushExtended().serialize():"null")+", ");a.u("angle: "+this.angle()+", ");a.u("marginLeft: "+this.marginLeft()+", ");a.u("marginRight: "+this.marginRight()+", ");a.u("marginTop: "+this.marginTop()+", ");a.u("marginBottom: "+this.marginBottom()+", ");a.u("opacity: "+this.opacity()+", ");a.u("visibility: "+(this.visibility()?"true":"false")+", ");if(this.horizontalAlignment()!=null){a.u('horizontalAlignment: "'+this.horizontalAlignment()+'", ')}if(this.verticalAlignment()!=null){a.u('verticalAlignment: "'+this.verticalAlignment()+'", ')}if(this.font()!=null){a.u('font: "'+this.font()+'",')}if(this.fontFamily()!=null){a.u('fontFamily: "'+$3(this.fontFamily(),'"',"'")+'",')}if(this.fontWeight()!=null){a.u('fontWeight: "'+this.fontWeight()+'",')}if(this.fontStyle()!=null){a.u('fontStyle: "'+this.fontStyle()+'",')}if(this.fontStretch()!=null){a.u('fontStretch: "'+this.fontStretch()+'",')}a.u("fontSize: "+this.fontSize());a.u("}");return a.toString()},$type:new $.ig.Type("LabelAppearanceData",$.ig.$ot,[$$t.$a.$type])},true);$c("VisualDataPixelScalingOptions:d","Object",{init:function(){$.ig.$op.init.call(this)},$type:new $.ig.Type("VisualDataPixelScalingOptions",$.ig.$ot)},true);$c("LabelAppearanceDataList:e","List$1",{init:function(){$$4.$x.init.call(this,$$t.$c.$type,0)},serialize:function(){var a=new $$6.aj(0);a.l("{ items: [");for(var b=0;b<this.count();b++){if(b!=0){a.l(", ")}a.l(this.__inner[b].serialize())}return a.toString()},$type:new $.ig.Type("LabelAppearanceDataList",$$4.$x.$type.specialize($$t.$c.$type),[$$t.$a.$type])},true);$c("BrushAppearanceData:f","Object",{init:function(){$.ig.$op.init.call(this)},type:function(){},serialize:function(){return'{ type: "'+this.type()+'", '+this.a()+" }"},a:function(){return""},$type:new $.ig.Type("BrushAppearanceData",$.ig.$ot,[$$t.$a.$type])},true);$c("ColorData:g","Object",{init:function(){$.ig.$op.init.call(this)},_a:0,a:function(a){if(arguments.length===1){this._a=a;return a}else{return this._a}},_r:0,r:function(a){if(arguments.length===1){this._r=a;return a}else{return this._r}},_g:0,g:function(a){if(arguments.length===1){this._g=a;return a}else{return this._g}},_b:0,b:function(a){if(arguments.length===1){this._b=a;return a}else{return this._b}},serialize:function(){return"{ a: "+this.a()+", r: "+this.r()+", g: "+this.g()+", b: "+this.b()+"}"},a_1:function(a){var b=new $$t.g;b.a(a.l());b.r(a.o());b.g(a.n());b.b(a.m());return b},f:function(){var a=new $$a.ax;a.l(this.a());a.o(this.r());a.n(this.g());a.m(this.b());return a},$type:new $.ig.Type("ColorData",$.ig.$ot,[$$t.$a.$type])},true);$c("RectData:h","Object",{init:function(a,b,c,d){$.ig.$op.init.call(this);this.left(a);this.top(b);this.width(c);this.height(d)},_top:0,top:function(a){if(arguments.length===1){this._top=a;return a}else{return this._top}},_left:0,left:function(a){if(arguments.length===1){this._left=a;return a}else{return this._left}},_width:0,width:function(a){if(arguments.length===1){this._width=a;return a}else{return this._width}},_height:0,height:function(a){if(arguments.length===1){this._height=a;return a}else{return this._height}},serialize:function(){return"{ top: "+this.top()+", left: "+this.left()+", width: "+this.width()+", height: "+this.height()+"}"},b:function(a){var b=new $$t.h(a.left(),a.top(),a.width(),a.height());return b},h:function(){var a=new $$a.ae(0,this.left(),this.top(),this.width(),this.height());return a},isEmpty:function(){return this.width()<0},empty:function(){return new $$t.h(Number.POSITIVE_INFINITY,Number.POSITIVE_INFINITY,Number.NEGATIVE_INFINITY,Number.NEGATIVE_INFINITY)},$type:new $.ig.Type("RectData",$.ig.$ot,[$$t.$a.$type])},true);$c("PointData:i","Object",{init:function(a,b){$.ig.$op.init.call(this);this.x(a);this.y(b)},_x:0,x:function(a){if(arguments.length===1){this._x=a;return a}else{return this._x}},_y:0,y:function(a){if(arguments.length===1){this._y=a;return a}else{return this._y}},serialize:function(){return"{ x: "+this.x()+", y: "+this.y()+"}"},b:function(a){var b=new $$t.i(a.__x,a.__y);return b},g:function(){var a=new $$a.y(0);a.__x=this.x();a.__y=this.y();return a},equals:function(a){var b=$b($$t.$i.$type,a);if(b==null){return false}return b.x()==this.x()&&b.y()==this.y()},getHashCode:function(){return this.x()*397^this.y()},empty:function(){return new $$t.i(Number.POSITIVE_INFINITY,Number.POSITIVE_INFINITY)},$type:new $.ig.Type("PointData",$.ig.$ot,[$$t.$a.$type])},true);$c("SizeData:j","Object",{init:function(a,b){$.ig.$op.init.call(this);this.width(a);this.height(b)},_width:0,width:function(a){if(arguments.length===1){this._width=a;return a}else{return this._width}},_height:0,height:function(a){if(arguments.length===1){this._height=a;return a}else{return this._height}},serialize:function(){return"{ width: "+this.width()+", height: "+this.height()+"}"},b:function(a){var b=new $$t.j(a.width(),a.height());return b},e:function(){var a=new $$a.af(1,this.width(),this.height());return a},empty:function(){return new $$t.j(Number.POSITIVE_INFINITY,Number.POSITIVE_INFINITY)},$type:new $.ig.Type("SizeData",$.ig.$ot,[$$t.$a.$type])},true);$c("SolidBrushAppearanceData:k","BrushAppearanceData",{init:function(){$$t.$f.init.call(this)},type:function(){return"solid"},_colorValue:null,colorValue:function(a){if(arguments.length===1){this._colorValue=a;return a}else{return this._colorValue}},a:function(){return"colorValue: "+(this.colorValue()!=null?this.colorValue().serialize():"null")},$type:new $.ig.Type("SolidBrushAppearanceData",$$t.$f.$type)},true);$c("LinearGradientBrushAppearanceData:l","BrushAppearanceData",{init:function(){$$t.$f.init.call(this);this.stops(new $$4.x($$t.$m.$type,0))},type:function(){return"linear"},_startX:0,startX:function(a){if(arguments.length===1){this._startX=a;return a}else{return this._startX}},_startY:0,startY:function(a){if(arguments.length===1){this._startY=a;return a}else{return this._startY}},_endX:0,endX:function(a){if(arguments.length===1){this._endX=a;return a}else{return this._endX}},_endY:0,endY:function(a){if(arguments.length===1){this._endY=a;return a}else{return this._endY}},_stops:null,stops:function(a){if(arguments.length===1){this._stops=a;return a}else{return this._stops}},a:function(){var a=new $$6.aj(0);a.l("startX: "+this.startX()+", endX: "+this.endX()+", startY: "+this.startY()+", endY: "+this.endY());a.l(", stops: [");for(var b=0;b<this.stops().count();b++){if(b>0){a.l(", ")}a.l(this.stops().__inner[b].serialize())}a.l("]");return a.toString()},$type:new $.ig.Type("LinearGradientBrushAppearanceData",$$t.$f.$type)},true);$c("GradientStopAppearanceData:m","Object",{init:function(){$.ig.$op.init.call(this)},_colorValue:null,colorValue:function(a){if(arguments.length===1){this._colorValue=a;return a}else{return this._colorValue}},_offset:0,offset:function(a){if(arguments.length===1){this._offset=a;return a}else{return this._offset}},serialize:function(){return"{ "+"colorValue: "+(this.colorValue()!=null?this.colorValue().serialize():"null")+", offset: "+this.offset()+" }"},$type:new $.ig.Type("GradientStopAppearanceData",$.ig.$ot,[$$t.$a.$type])},true);$c("PrimitiveAppearanceData:n","Object",{init:function(){$.ig.$op.init.call(this)},_stroke:null,stroke:function(a){if(arguments.length===1){this._stroke=a;return a}else{return this._stroke}},_strokeExtended:null,strokeExtended:function(a){if(arguments.length===1){this._strokeExtended=a;return a}else{return this._strokeExtended}},_fill:null,fill:function(a){if(arguments.length===1){this._fill=a;return a}else{return this._fill}},_fillExtended:null,fillExtended:function(a){if(arguments.length===1){this._fillExtended=a;return a}else{return this._fillExtended}},_strokeThickness:0,strokeThickness:function(a){if(arguments.length===1){this._strokeThickness=a;return a}else{return this._strokeThickness}},_isVisible:false,isVisible:function(a){if(arguments.length===1){this._isVisible=a;return a}else{return this._isVisible}},_opacity:0,opacity:function(a){if(arguments.length===1){this._opacity=a;return a}else{return this._opacity}},_canvasLeft:0,canvasLeft:function(a){if(arguments.length===1){this._canvasLeft=a;return a}else{return this._canvasLeft}},_canvasTop:0,canvasTop:function(a){if(arguments.length===1){this._canvasTop=a;return a}else{return this._canvasTop}},_canvaZIndex:0,canvaZIndex:function(a){if(arguments.length===1){this._canvaZIndex=a;return a}else{return this._canvaZIndex}},_dashArray:null,dashArray:function(a){if(arguments.length===1){this._dashArray=a;return a}else{return this._dashArray}},_dashCap:0,dashCap:function(a){if(arguments.length===1){this._dashCap=a;return a}else{return this._dashCap}},m:function(a){this.canvasLeft((this.canvasLeft()-a.left())/a.width());this.canvasTop((this.canvasTop()-a.top())/a.height())},serialize:function(){var a=new $$6.aj(0);a.u("{");a.u("stroke: "+(this.stroke()!=null?this.stroke().serialize():"null")+", ");a.u("fill: "+(this.fill()!=null?this.fill().serialize():"null")+", ");a.u("strokeExtended: "+(this.strokeExtended()!=null?this.strokeExtended().serialize():"null")+", ");a.u("fillExtended: "+(this.fillExtended()!=null?this.fillExtended().serialize():"null")+", ");a.u("strokeThickness: "+this.strokeThickness()+", ");a.u("isVisible: "+(this.isVisible()?"true":"false")+", ");a.u("opacity: "+this.opacity()+", ");a.u("canvasLeft: "+this.canvasLeft()+", ");a.u("canvasTop: "+this.canvasTop()+", ");a.u("canvasZIndex: "+this.canvaZIndex()+", ");a.u("dashArray: null, ");a.u("dashCap: "+this.dashCap());a.u("}");return a.toString()},$type:new $.ig.Type("PrimitiveAppearanceData",$.ig.$ot,[$$t.$a.$type])},true);$c("GetPointsSettings:o","Object",{init:function(){$.ig.$op.init.call(this)},_ignoreFigureStartPoint:false,ignoreFigureStartPoint:function(a){if(arguments.length===1){this._ignoreFigureStartPoint=a;return a}else{return this._ignoreFigureStartPoint}},$type:new $.ig.Type("GetPointsSettings",$.ig.$ot)},true);$c("RectangleVisualData:q","PrimitiveVisualData",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$p.init1.call(this,1,"rect1")},_width:0,width:function(a){if(arguments.length===1){this._width=a;return a}else{return this._width}},_height:0,height:function(a){if(arguments.length===1){this._height=a;return a}else{return this._height}},_radiusX:0,radiusX:function(a){if(arguments.length===1){this._radiusX=a;return a}else{return this._radiusX}},_radiusY:0,radiusY:function(a){if(arguments.length===1){this._radiusY=a;return a}else{return this._radiusY}},e:function(){return"width: "+this.width()+", height: "+this.height()+", radiusX: "+this.radiusX()+", radiusY: "+this.radiusY()},init1:function(a,b,c){$$t.$p.init1.call(this,1,b);this.width(c.width());this.height(c.height());this.radiusX(c.al());this.radiusY(c.am());$$t.$ai.p(this.appearance(),c)},type:function(){return"Rectangle"},scaleByViewport:function(a){$$t.$p.scaleByViewport.call(this,a);this.width(this.width()/a.width());this.height(this.height()/a.height())},getPointsOverride:function(a,b){var c=new $$4.x($$t.$i.$type,0);a.add(c);c.add(new $$t.i(this.appearance().canvasLeft(),this.appearance().canvasTop()));c.add(new $$t.i(this.appearance().canvasLeft()+this.width(),this.appearance().canvasTop()));c.add(new $$t.i(this.appearance().canvasLeft()+this.width(),this.appearance().canvasTop()+this.height()));c.add(new $$t.i(this.appearance().canvasLeft(),this.appearance().canvasTop()+this.height()))},$type:new $.ig.Type("RectangleVisualData",$$t.$p.$type)},true);$c("ShapeTags:r","List$1",{init:function(){$$4.$x.init.call(this,String,0)},$type:new $.ig.Type("ShapeTags",$$4.$x.$type.specialize(String))},true);$c("LineVisualData:s","PrimitiveVisualData",{type:function(){return"Line"},init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$p.init1.call(this,1,"line1")},init1:function(a,b,c){$$t.$p.init1.call(this,1,b);this.x1(c._aj);this.y1(c._al);this.x2(c._ak);this.y2(c._am);$$t.$ai.p(this.appearance(),c)},_x1:0,x1:function(a){if(arguments.length===1){this._x1=a;return a}else{return this._x1}},_y1:0,y1:function(a){if(arguments.length===1){this._y1=a;return a}else{return this._y1}},_x2:0,x2:function(a){if(arguments.length===1){this._x2=a;return a}else{return this._x2}},_y2:0,y2:function(a){if(arguments.length===1){this._y2=a;return a}else{return this._y2}},e:function(){return"x1: "+this.x1()+", y1: "+this.y1()+", x2: "+this.x2()+", y2: "+this.y2()},scaleByViewport:function(a){$$t.$p.scaleByViewport.call(this,a);this.x1((this.x1()-a.left())/a.width());this.y1((this.y1()-a.top())/a.height())},getPointsOverride:function(a,b){var c=new $$4.x($$t.$i.$type,0);a.add(c);c.add(new $$t.i(this.x1(),this.y1()));c.add(new $$t.i(this.x2(),this.y2()))},$type:new $.ig.Type("LineVisualData",$$t.$p.$type)},true);$c("PolyLineVisualData:t","PrimitiveVisualData",{type:function(){return"Polyline"},init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$p.init1.call(this,1,"polyLine1");this.points(new $$4.x($$t.$i.$type,0))},init1:function(a,b,c){$$t.$p.init1.call(this,1,b);this.points(new $$4.x($$t.$i.$type,0));for(var d=0;d<c._aj.count();d++){this.points().add($$t.$i.b(c._aj.__inner[d]))}$$t.$ai.p(this.appearance(),c)},_points:null,points:function(a){if(arguments.length===1){this._points=a;return a}else{return this._points}},e:function(){var a=new $$6.aj(0);a.u("points: [");for(var b=0;b<this.points().count();b++){if(b!=0){a.l(", ")}a.l("{ x: "+this.points().__inner[b].x()+", y: "+this.points().__inner[b].y()+"}")}a.u("]");return a.toString()},scaleByViewport:function(a){$$t.$p.scaleByViewport.call(this,a);for(var b=0;b<this.points().count();b++){this.points().__inner[b]=new $$t.i((this.points().__inner[b].x()-a.left())/a.width(),(this.points().__inner[b].y()-a.top())/a.height())}},getPointsOverride:function(a,b){var c=new $$4.x($$t.$i.$type,0);a.add(c);for(var d=0;d<this.points().count();d++){c.add(this.points().__inner[d])}},$type:new $.ig.Type("PolyLineVisualData",$$t.$p.$type)},true);$c("PolygonVisualData:u","PrimitiveVisualData",{type:function(){return"Polygon"},init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$p.init1.call(this,1,"polygon1");this.points(new $$4.x($$t.$i.$type,0))},init1:function(a,b,c){$$t.$p.init1.call(this,1,b);this.points(new $$4.x($$t.$i.$type,0));for(var d=0;d<c._aj.count();d++){this.points().add($$t.$i.b(c._aj.__inner[d]))}$$t.$ai.p(this.appearance(),c)},_points:null,points:function(a){if(arguments.length===1){this._points=a;return a}else{return this._points}},e:function(){var a=new $$6.aj(0);a.u("points: [");for(var b=0;b<this.points().count();b++){if(b!=0){a.l(", ")}a.l("{ x: "+this.points().__inner[b].x()+", y: "+this.points().__inner[b].y()+"}")}a.u("]");return a.toString()},scaleByViewport:function(a){$$t.$p.scaleByViewport.call(this,a);for(var b=0;b<this.points().count();b++){this.points().__inner[b]=new $$t.i((this.points().__inner[b].x()-a.left())/a.width(),(this.points().__inner[b].y()-a.top())/a.height())}},getPointsOverride:function(a,b){var c=new $$4.x($$t.$i.$type,0);a.add(c);for(var d=0;d<this.points().count();d++){c.add(this.points().__inner[d])}},$type:new $.ig.Type("PolygonVisualData",$$t.$p.$type)},true);$c("PathVisualData:v","PrimitiveVisualData",{type:function(){return"Path"},init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break;case 2:this.init2.apply(this,arguments);break}return}$$t.$p.init1.call(this,1,"path1");this.data(new $$4.x($$t.$w.$type,0))},init1:function(a,b,c){$$t.$p.init1.call(this,1,b);this.data($$t.$ai.g(c._aj));$$t.$ai.p(this.appearance(),c)},init2:function(a,b,c){$$t.$p.init1.call(this,1,b);this.data($$t.$ai.h(c));$$t.$ai.p(this.appearance(),c)},_data:null,data:function(a){if(arguments.length===1){this._data=a;return a}else{return this._data}},e:function(){var a=new $$6.aj(0);a.u("data: [");for(var b=0;b<this.data().count();b++){if(b!=0){a.l(", ")}a.l(this.data().__inner[b]!=null?this.data().__inner[b].serialize():"null")}a.u("]");return a.toString()},scaleByViewport:function(a){$$t.$p.scaleByViewport.call(this,a);var c=this.data().getEnumerator();while(c.moveNext()){var b=c.current();b.scaleByViewport(a)}},getPointsOverride:function(a,b){for(var c=0;c<this.data().count();c++){var d=this.data().__inner[c];d.getPointsOverride(a,b)}},$type:new $.ig.Type("PathVisualData",$$t.$p.$type)},true);$c("GeometryData:w","Object",{init:function(){$.ig.$op.init.call(this)},type:function(){},scaleByViewport:function(a){},getPointsOverride:function(a,b){},serialize:function(){return'{ type: "'+this.type()+'", '+this.a()+"}"},a:function(){return""},$type:new $.ig.Type("GeometryData",$.ig.$ot,[$$t.$a.$type])},true);$c("PathGeometryData:x","GeometryData",{init:function(){$$t.$w.init.call(this);this.figures(new $$4.x($$t.$ab.$type,0))},type:function(){return"Path"},_figures:null,figures:function(a){if(arguments.length===1){this._figures=a;return a}else{return this._figures}},a:function(){var a=new $$6.aj(0);a.u("figures: [");for(var b=0;b<this.figures().count();b++){if(b!=0){a.l(", ")}a.l(this.figures().__inner[b].serialize())}a.u("]");return a.toString()},scaleByViewport:function(a){var c=this.figures().getEnumerator();while(c.moveNext()){var b=c.current();b.d(a)}},getPointsOverride:function(a,b){for(var c=0;c<this.figures().count();c++){var d=this.figures().__inner[c];d.getPointsOverride(a,b)}},$type:new $.ig.Type("PathGeometryData",$$t.$w.$type)},true);$c("LineGeometryData:y","GeometryData",{init:function(){$$t.$w.init.call(this)},type:function(){return"Line"},_x1:0,x1:function(a){if(arguments.length===1){this._x1=a;return a}else{return this._x1}},_y1:0,y1:function(a){if(arguments.length===1){this._y1=a;return a}else{return this._y1}},_x2:0,x2:function(a){if(arguments.length===1){this._x2=a;return a}else{return this._x2}},_y2:0,y2:function(a){if(arguments.length===1){this._y2=a;return a}else{return this._y2}},a:function(){return"x1: "+this.x1()+", y1: "+this.y1()+", x2: "+this.x2()+", y2:"+this.y2()},scaleByViewport:function(a){this.x1((this.x1()-a.left())/a.width());this.y1((this.y1()-a.top())/a.height());this.x2((this.x2()-a.left())/a.width());this.y2((this.y2()-a.top())/a.height())},getPointsOverride:function(a,b){var c=new $$4.x($$t.$i.$type,0);a.add(c);c.add(new $$t.i(this.x1(),this.y1()));c.add(new $$t.i(this.x2(),this.y2()))},$type:new $.ig.Type("LineGeometryData",$$t.$w.$type)},true);$c("RectangleGeometryData:z","GeometryData",{init:function(){$$t.$w.init.call(this)},type:function(){return"Rectangle"},_x:0,x:function(a){if(arguments.length===1){this._x=a;return a}else{return this._x}},_y:0,y:function(a){if(arguments.length===1){this._y=a;return a}else{return this._y}},_width:0,width:function(a){if(arguments.length===1){this._width=a;return a}else{return this._width}},_height:0,height:function(a){if(arguments.length===1){this._height=a;return a}else{return this._height}},a:function(){return"x: "+this.x()+", y: "+this.y()+", width: "+this.width()+", height: "+this.height()},scaleByViewport:function(a){this.x((this.x()-a.left())/a.width());this.y((this.y()-a.top())/a.height());this.width(this.width()/a.width());this.height(this.height()/a.height())},getPointsOverride:function(a,b){var c=new $$4.x($$t.$i.$type,0);a.add(c);c.add(new $$t.i(this.x(),this.y()));c.add(new $$t.i(this.x()+this.width(),this.y()));c.add(new $$t.i(this.x()+this.width(),this.y()+this.height()));c.add(new $$t.i(this.x(),this.y()+this.height()))},$type:new $.ig.Type("RectangleGeometryData",$$t.$w.$type)},true);$c("EllipseGeometryData:aa","GeometryData",{init:function(){$$t.$w.init.call(this)},type:function(){return"Ellipse"},_centerX:0,centerX:function(a){if(arguments.length===1){this._centerX=a;return a}else{return this._centerX}},_centerY:0,centerY:function(a){if(arguments.length===1){this._centerY=a;return a}else{return this._centerY}},_radiusX:0,radiusX:function(a){if(arguments.length===1){this._radiusX=a;return a}else{return this._radiusX}},_radiusY:0,radiusY:function(a){if(arguments.length===1){this._radiusY=a;return a}else{return this._radiusY}},a:function(){return"centerX: "+this.centerX()+", centerY: "+this.centerY()+", radiusX: "+this.radiusX()+", radiusY: "+this.radiusY()},scaleByViewport:function(a){this.centerX((this.centerX()-a.left())/a.width());this.centerY((this.centerY()-a.top())/a.height());this.radiusX(this.radiusX()/a.width());this.radiusY(this.radiusY()/a.height())},getPointsOverride:function(a,b){var c=new $$4.x($$t.$i.$type,0);a.add(c);c.add(new $$t.i(this.centerX(),this.centerY()))},$type:new $.ig.Type("EllipseGeometryData",$$t.$w.$type)},true);$c("PathFigureData:ab","Object",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$.ig.$op.init.call(this);this.segments(new $$4.x($$t.$ac.$type,0));this.startPoint(new $$t.i(NaN,NaN))},init1:function(a,b){$.ig.$op.init.call(this);this.segments(new $$4.x($$t.$ac.$type,0));this.startPoint($$t.$i.b(b.__startPoint));for(var c=0;c<b.__segments.count();c++){var d=b.__segments.__inner[c];var e=null;switch(d.a()){case 0:e=new $$t.ad(1,d);break;case 3:e=new $$t.ae(1,d);break;case 4:e=new $$t.ah(1,d);break;case 2:e=new $$t.ag(1,d);break;case 1:e=new $$t.af(1,d);break}this.segments().add(e)}},_startPoint:null,startPoint:function(a){if(arguments.length===1){this._startPoint=a;return a}else{return this._startPoint}},_segments:null,segments:function(a){if(arguments.length===1){this._segments=a;return a}else{return this._segments}},serialize:function(){var a=new $$6.aj(0);a.u("{");if(this.startPoint()!=null){a.u("startPoint: { x: "+this.startPoint().x()+", y: "+this.startPoint().y()+"}, ")}a.u("segments: [");for(var b=0;b<this.segments().count();b++){if(b!=0){a.l(", ")}a.l(this.segments().__inner[b].serialize())}a.u("]");a.u("}");return a.toString()},d:function(a){if(this.startPoint()!=null){this.startPoint(new $$t.i((this.startPoint().x()-a.left())/a.width(),(this.startPoint().y()-a.top())/a.height()))}for(var b=0;b<this.segments().count();b++){this.segments().__inner[b].scaleByViewport(a)}},getPointsOverride:function(a,b){var c=new $$4.x($$t.$i.$type,0);a.add(c);if(!b.ignoreFigureStartPoint()){c.add(new $$t.i(this.startPoint().x(),this.startPoint().y()))}for(var d=0;d<this.segments().count();d++){this.segments().__inner[d].getPointsOverride(c,b)}},$type:new $.ig.Type("PathFigureData",$.ig.$ot,[$$t.$a.$type])},true);$c("SegmentData:ac","Object",{init:function(){$.ig.$op.init.call(this)},type:function(){},scaleByViewport:function(a){},getPointsOverride:function(a,b){},serialize:function(){var a=new $$6.aj(0);a.u("{");a.u('type: "'+this.type()+'", ');a.u(this.a());a.u("}");return a.toString()},a:function(){return""},$type:new $.ig.Type("SegmentData",$.ig.$ot,[$$t.$a.$type])},true);$c("LineSegmentData:ad","SegmentData",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$ac.init.call(this);this.point(new $$t.i(NaN,NaN))},init1:function(a,b){$$t.$ac.init.call(this);this.point($$t.$i.b(b.c()))},type:function(){return"Line"},_point:null,point:function(a){if(arguments.length===1){this._point=a;return a}else{return this._point}},a:function(){return"point: { x: "+this.point().x()+", y: "+this.point().y()+"}"},scaleByViewport:function(a){this.point(new $$t.i((this.point().x()-a.left())/a.width(),(this.point().y()-a.top())/a.height()))},getPointsOverride:function(a,b){a.add(new $$t.i(this.point().x(),this.point().y()))},$type:new $.ig.Type("LineSegmentData",$$t.$ac.$type)},true);$c("PolylineSegmentData:ae","SegmentData",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$ac.init.call(this);this.points(new $$4.x($$t.$i.$type,0))},init1:function(a,b){$$t.$ac.init.call(this);this.points(new $$4.x($$t.$i.$type,0));for(var c=0;c<b.__points.count();c++){this.points().add($$t.$i.b(b.__points.__inner[c]))}},type:function(){return"Polyline"},_points:null,points:function(a){if(arguments.length===1){this._points=a;return a}else{return this._points}},a:function(){var a=new $$6.aj(0);a.u("points: [");for(var b=0;b<this.points().count();b++){if(b!=0){a.l(", ")}a.l("{ x: "+this.points().__inner[b].x()+", y: "+this.points().__inner[b].y()+"}")}a.u("]");return a.toString()},scaleByViewport:function(a){for(var b=0;b<this.points().count();b++){this.points().__inner[b]=new $$t.i((this.points().__inner[b].x()-a.left())/a.width(),(this.points().__inner[b].y()-a.top())/a.height())}},getPointsOverride:function(a,b){for(var c=0;c<this.points().count();c++){a.add(new $$t.i(this.points().__inner[c].x(),this.points().__inner[c].y()))}},$type:new $.ig.Type("PolylineSegmentData",$$t.$ac.$type)},true);$c("BezierSegmentData:af","SegmentData",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$ac.init.call(this);this.points(new $$4.x($$t.$i.$type,0))},init1:function(a,b){$$t.$ac.init.call(this);this.points(new $$4.x($$t.$i.$type,0));this.points().add($$t.$i.b(b.e()));this.points().add($$t.$i.b(b.f()));this.points().add($$t.$i.b(b.g()))},type:function(){return"Bezier"},_points:null,points:function(a){if(arguments.length===1){this._points=a;return a}else{return this._points}},a:function(){var a=new $$6.aj(0);a.u("points: [");for(var b=0;b<this.points().count();b++){if(this.points().__inner[b]==null){break}if(b!=0){a.l(", ")}a.l("{ x: "+this.points().__inner[b].x()+", y: "+this.points().__inner[b].y()+"}")}a.u("]");return a.toString()},scaleByViewport:function(a){for(var b=0;b<this.points().count();b++){if(this.points().__inner[b]==null){break}this.points().__inner[b]=new $$t.i((this.points().__inner[b].x()-a.left())/a.width(),(this.points().__inner[b].y()-a.top())/a.height());
}},getPointsOverride:function(a,b){for(var c=0;c<this.points().count();c++){a.add(new $$t.i(this.points().__inner[c].x(),this.points().__inner[c].y()))}},$type:new $.ig.Type("BezierSegmentData",$$t.$ac.$type)},true);$c("PolyBezierSegmentData:ag","SegmentData",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$ac.init.call(this);this.points(new $$4.x($$t.$i.$type,0))},init1:function(a,b){$$t.$ac.init.call(this);this.points(new $$4.x($$t.$i.$type,0));for(var c=0;c<b._b.count();c++){this.points().add($$t.$i.b(b._b.__inner[c]))}},type:function(){return"PolyBezierSpline"},_points:null,points:function(a){if(arguments.length===1){this._points=a;return a}else{return this._points}},a:function(){var a=new $$6.aj(0);a.u("points: [");for(var b=0;b<this.points().count();b++){if(b!=0){a.l(", ")}a.l("{ x: "+this.points().__inner[b].x()+", y: "+this.points().__inner[b].y()+"}")}a.u("]");return a.toString()},scaleByViewport:function(a){for(var b=0;b<this.points().count();b++){this.points().__inner[b]=new $$t.i((this.points().__inner[b].x()-a.left())/a.width(),(this.points().__inner[b].y()-a.top())/a.height())}},getPointsOverride:function(a,b){for(var c=0;c<this.points().count();c++){a.add(new $$t.i(this.points().__inner[c].x(),this.points().__inner[c].y()))}},$type:new $.ig.Type("PolyBezierSegmentData",$$t.$ac.$type)},true);$c("ArcSegmentData:ah","SegmentData",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$t.$ac.init.call(this);this.point(new $$t.i(NaN,NaN));this.isLargeArc(false);this.isCounterClockwise(true);this.rotationAngle(0)},init1:function(a,b){$$t.$ac.init.call(this);this.point($$t.$i.b(b._e));this.isLargeArc(b._b);this.isCounterClockwise(b._d==0);this.sizeX(b._f.width());this.sizeY(b._f.height());this.rotationAngle(b._c)},type:function(){return"Arc"},_point:null,point:function(a){if(arguments.length===1){this._point=a;return a}else{return this._point}},_isLargeArc:false,isLargeArc:function(a){if(arguments.length===1){this._isLargeArc=a;return a}else{return this._isLargeArc}},_isCounterClockwise:false,isCounterClockwise:function(a){if(arguments.length===1){this._isCounterClockwise=a;return a}else{return this._isCounterClockwise}},_sizeX:0,sizeX:function(a){if(arguments.length===1){this._sizeX=a;return a}else{return this._sizeX}},_sizeY:0,sizeY:function(a){if(arguments.length===1){this._sizeY=a;return a}else{return this._sizeY}},_rotationAngle:0,rotationAngle:function(a){if(arguments.length===1){this._rotationAngle=a;return a}else{return this._rotationAngle}},a:function(){return"point: { x: "+this.point().x()+", y: "+this.point().y()+" }, isLargeArc: "+(this.isLargeArc()?"true":"false")+", isCounterClockwise: "+(this.isCounterClockwise()?"true":"false")+", sizeX: "+this.sizeX()+", sizeY: "+this.sizeY()+", rotationAngle: "+this.rotationAngle()},scaleByViewport:function(a){this.point(new $$t.i((this.point().x()-a.left())/a.width(),(this.point().y()-a.top())/a.height()));this.sizeX(this.sizeX()/a.width());this.sizeY(this.sizeY()/a.height())},getPointsOverride:function(a,b){a.add(new $$t.i(this.point().x(),this.point().y()))},$type:new $.ig.Type("ArcSegmentData",$$t.$ac.$type)},true);$c("AppearanceHelper:ai","Object",{init:function(){$.ig.$op.init.call(this)},b:function(a){if(a==null){return $$t.$g.a_1($$a.$ax.u(0,0,0,0))}if($$a.$ax.c($m($$a.$ax.$type,a.color()),$m($$a.$ax.$type,null))){return $$t.$g.a_1($$a.$ax.u(0,0,0,0))}return $$t.$g.a_1(a.color())},a:function(a){if(a==null){return null}if(a._isGradient){var b=new $$t.l;var c=a;b.startX(c._startX);b.startY(c._startY);b.endX(c._endX);b.endY(c._endY);var f=c._gradientStops;for(var e=0;e<f.length;e++){var d=f[e];var g=new $$t.m;g.colorValue($$t.$g.a_1(d.color()));g.offset(d._offset);b.stops().add(g)}return b}else if(a._isRadialGradient){return null}else{var h=new $$t.k;h.colorValue($$t.$g.a_1(a.color()));return h}},m:function(a){return a._n},n:function(a){return a._o},o:function(a){return a._s},j:function(a){return $$t.$ai.g(a._aj)},h:function(a){var b=new $$a.a4;b._b={__x:a._ak,__y:a._am,$type:$$a.$y.$type,getType:$.ig.$op.getType,getGetHashCode:$.ig.$op.getGetHashCode,typeName:$.ig.$op.typeName};b._c={__x:a._aj,__y:a._al,$type:$$a.$y.$type,getType:$.ig.$op.getType,getGetHashCode:$.ig.$op.getGetHashCode,typeName:$.ig.$op.typeName};return $$t.$ai.g(b)},g:function(a){if(a==null){return new $$4.x($$t.$w.$type,0)}if($b($$a.$a3.$type,a)!==null){var b=new $$4.x($$t.$w.$type,0);var c=a;for(var d=0;d<c._c.count();d++){var e=$$t.$ai.g(c._c.__inner[d]);for(var f=0;f<e.count();f++){b.add(e.__inner[f])}}return b}else if($b($$a.$a7.$type,a)!==null){return $$t.$ai.k(a)}else if($b($$a.$a4.$type,a)!==null){return $$t.$ai.i(a)}else if($b($$a.$a5.$type,a)!==null){return $$t.$ai.l(a)}else if($b($$a.$a6.$type,a)!==null){return $$t.$ai.f(a)}else{throw new $$0.n(1,"not supported")}},f:function(a){var b=new $$4.x($$t.$w.$type,0);var c=new $$t.aa;b.add(c);c.centerX(a._d.__x);c.centerY(a._d.__y);c.radiusX(a._b);c.radiusY(a._c);return b},l:function(a){var b=new $$4.x($$t.$w.$type,0);var c=new $$t.z;b.add(c);c.x(a._d.x());c.y(a._d.y());c.width(a._d.width());c.height(a._d.height());return b},i:function(a){var b=new $$4.x($$t.$w.$type,0);var c=new $$t.y;b.add(c);c.x1(a._c.__x);c.y1(a._c.__y);c.x2(a._b.__x);c.y2(a._b.__y);return b},k:function(a){var b=new $$4.x($$t.$w.$type,0);var c=new $$t.x;b.add(c);for(var d=0;d<a._b.count();d++){var e=a._b.__inner[d];var f=new $$t.ab(1,e);c.figures().add(f)}return b},p:function(a,b){a.stroke($$t.$ai.b(b.__stroke));a.fill($$t.$ai.b(b.__fill));a.strokeExtended($$t.$ai.a(b.__stroke));a.fillExtended($$t.$ai.a(b.__fill));a.strokeThickness(b._ac);a.dashArray(null);if(b._ai!=null){a.dashArray(b._ai.asArray())}a.dashCap(b._ad);a.isVisible(b.__visibility==0);a.opacity(b.__opacity);a.canvasLeft($$t.$ai.m(b));a.canvasTop($$t.$ai.n(b));a.canvaZIndex($$t.$ai.o(b))},c:function(a,b){var c=new $$t.c;var d=a;c.text(d.ak());c.labelBrush($$t.$ai.b(d._am));c.labelBrushExtended($$t.$ai.a(d._am));c.visibility(d.__visibility==0?true:false);c.opacity(d.__opacity);if(b!=null){if(b.n()!=null){c.fontFamily(b.n())}if(!$.ig.util.isNaN(b.d())){c.fontSize(b.d())}if(b.s()!=null){c.fontWeight(b.s())}if(b.q()!=null){c.fontStyle(b.q())}if(b.o()!=null){c.fontStretch(b.q())}}var e=0;var f=d._j;if($b($$a.$bm.$type,f)!==null){var g=$b($$a.$bm.$type,f);e=g._j}else if($b($$a.$bp.$type,f)!==null){var h=$b($$a.$bp.$type,f);var j=h._j.getEnumerator();while(j.moveNext()){var i=j.current();if($b($$a.$bm.$type,i)!==null){var k=$b($$a.$bm.$type,i);e=k._j;break}}}c.angle(e);return c},serializeItems:function(a,b,c,d){if(c!=null){if(!d){a.l(", ")}a.l(b);a.l(": [");var e=false;var g=c.getEnumerator();while(g.moveNext()){var f=g.current();if(e){a.u(", ")}else{e=true}a.l(f.serialize())}a.u("]");return true}return false},serializeItem:function(a,b,c,d){if(c!=null){if(!d){a.l(", ")}a.l(b);a.l(": ");a.u(c.serialize());return true}return false},$type:new $.ig.Type("AppearanceHelper",$.ig.$ot)},true)});(function(factory){if(typeof define==="function"&&define.amd){define("watermark",["jquery"],factory)}else{factory(jQuery)}})(function($){$(document).ready(function(){var wm=$("#__ig_wm__").length>0?$("#__ig_wm__"):$("<div id='__ig_wm__'></div>").appendTo(document.body);wm.css({position:"fixed",bottom:0,right:0,zIndex:1e3}).addClass("ui-igtrialwatermark")})});