/*!@license
* Infragistics.Web.ClientUI infragistics.ext_collections.js 19.1.20191.376
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
*/
(function(factory){if(typeof define==="function"&&define.amd){define(["./infragistics.util","./infragistics.ext_core"],factory)}else{factory(igRoot)}})(function($){$.ig=$.ig||{};var $$t={};$.ig.globalDefs=$.ig.globalDefs||{};$.ig.globalDefs.$$4=$$t;$$0=$.ig.globalDefs.$$0;$$1=$.ig.globalDefs.$$1;$$6=$.ig.globalDefs.$$6;$.ig.$currDefinitions=$$t;$.ig.util.bulkDefine(["EqualityComparer$1:m","DefaultEqualityComparer$1:n","IArrayList:r","IArray:s","IDictionary$2:t","INotifyCollectionChanged:ac","NotifyCollectionChangedEventHandler:ad"]);var $a=$.ig.intDivide,$b=$.ig.util.cast,$c=$.ig.util.defType,$d=$.ig.util.defEnum,$e=$.ig.util.getBoxIfEnum,$f=$.ig.util.getDefaultValue,$g=$.ig.util.getEnumValue,$h=$.ig.util.getValue,$i=$.ig.util.intSToU,$j=$.ig.util.nullableEquals,$k=$.ig.util.nullableIsNull,$l=$.ig.util.nullableNotEquals,$m=$.ig.util.toNullable,$n=$.ig.util.toString$1,$o=$.ig.util.u32BitwiseAnd,$p=$.ig.util.u32BitwiseOr,$q=$.ig.util.u32BitwiseXor,$r=$.ig.util.u32LS,$s=$.ig.util.unwrapNullable,$t=$.ig.util.wrapNullable,$u=String.fromCharCode,$v=$.ig.util.castObjTo$t,$w=$.ig.util.compareSimple,$x=$.ig.util.compare,$y=$.ig.util.replace,$z=$.ig.util.stringFormat,$0=$.ig.util.stringFormat1,$1=$.ig.util.stringFormat2,$2=$.ig.util.stringCompare1,$3=$.ig.util.stringCompare2,$4=$.ig.util.stringCompare3;$d("NotifyCollectionChangedAction:af",false,false,{Add:0,Remove:1,Replace:2,Reset:4});$c("Buffer:a","Object",{init:function(){$.ig.$op.init.call(this)},b:function(a){try{}catch(b){}if(a==null){throw new $$6.e(0,"array")}var c=a.length;if(c<0){throw new $$6.d(1,"Object must be an array of primitives.")}return c},a:function(a,b){if(b<0||b>=$$t.$a.b(a)){throw new $$6.f(2,"index","Value must be non-negative and less than the size of the collection.")}return $h(a[b])},d:function(a,b,c){if(b<0||b>=$$t.$a.b(a)){throw new $$6.f(2,"index","Value must be non-negative and less than the size of the collection.")}a[b]=c},c:function(a,b,c,d,e){if(a==null){throw new $$6.e(0,"src")}if(c==null){throw new $$6.e(0,"dst")}if(b<0){throw new $$6.f(2,"srcOffset","Non-negative number required.")}if(d<0){throw new $$6.f(2,"dstOffset","Non-negative number required.")}if(e<0){throw new $$6.f(2,"count","Non-negative number required.")}if(b>$$t.$a.b(a)-e||d>$$t.$a.b(c)-e){throw new $$6.d(1,"Offset and length were out of bounds for the array or count is greater than "+"the number of elements from index to the end of the source collection.")}for(var f=0;f<e;f++){c[d++]=a[b++]}},$type:new $.ig.Type("Buffer",$.ig.$ot)},true);$c("AbstractEnumerable:b","Object",{a:null,init:function(a){$.ig.$op.init.call(this);this.a=a},getEnumerator:function(){return new $$t.c(this.a().getEnumerator())},$type:new $.ig.Type("AbstractEnumerable",$.ig.$ot,[$$0.$bt.$type])},true);$c("AbstractEnumerator:c","Object",{a:null,init:function(a){$.ig.$op.init.call(this);this.a=a},current:function(){return this.a.current()},moveNext:function(){return this.a.moveNext()},reset:function(){this.a.reset()},$type:new $.ig.Type("AbstractEnumerator",$.ig.$ot,[$$0.$bu.$type])},true);$c("INotifyCollectionChanged:ac","Object",{$type:new $.ig.Type("INotifyCollectionChanged",null)},true);$c("NotifyCollectionChangedEventArgs:ae","EventArgs",{init:function(a,b){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break;case 2:this.init2.apply(this,arguments);break}return}$$0.$w.init.call(this);this.f=b;this.c=new $$t.x($.ig.$op.$type,0);this.b=new $$t.x($.ig.$op.$type,0)},init1:function(a,b,c,d){$$0.$w.init.call(this);this.f=b;this.c=new $$t.x($.ig.$op.$type,0);if(this.f==1||this.f==2){this.c.add(c);this.i=d}if(this.f!=1){this.b=function(){var $ret=new $$t.x($.ig.$op.$type,0);$ret.add1(c);return $ret}()}else{this.b=new $$t.x($.ig.$op.$type,0)}this.h=d},init2:function(a,b,c,d,e){$$0.$w.init.call(this);this.f=b;this.h=e;this.i=e;this.b=function(){var $ret=new $$t.x($.ig.$op.$type,0);$ret.add1(c);return $ret}();this.c=function(){var $ret=new $$t.x($.ig.$op.$type,0);$ret.add1(d);return $ret}()},f:0,action:function(){return this.f},b:null,newItems:function(){return this.b},h:0,newStartingIndex:function(){return this.h},c:null,oldItems:function(){return this.c},i:0,oldStartingIndex:function(){return this.i},$type:new $.ig.Type("NotifyCollectionChangedEventArgs",$$0.$w.$type)},true);$c("NameValueCollection:ag","Object",{a:null,init:function(){$.ig.$op.init.call(this);this.a=new $$t.h(String,String,0)},item:function(a,b){if(arguments.length===2){if(this.a.containsKey(a)){this.a.item(a,b)}else{this.a.add(a,b)}return b}else{if(this.a.containsKey(a)){return this.a.item(a)}else{return null}}},$type:new $.ig.Type("NameValueCollection",$.ig.$ot)},true);$c("KeyValuePair$2:u","ValueType",{$tKey:null,$tValue:null,init:function($tKey,$tValue,a){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$$0.$bh.init.call(this)},a:null,c:null,init1:function($tKey,$tValue,a,b,c){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}$$0.$bh.init.call(this);this.a=b;this.c=c},key:function(){return this.a},value:function(){return this.c},$type:new $.ig.Type("KeyValuePair$2",$$0.$bh.$type)},true);$c("IDictionary$2:t","Object",{$type:new $.ig.Type("IDictionary$2",null,[$$0.$bv.$type.specialize($$t.$u.$type.specialize(0,1)),$$0.$bw.$type.specialize($$t.$u.$type.specialize(0,1)),$$0.$bt.$type])},true);$c("Dictionary$2:h","Object",{$tKey:null,$tValue:null,j:null,k:0,e:false,d:false,c:false,a:null,b:null,init:function($tKey,$tValue,a){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break;case 2:this.init2.apply(this,arguments);break;case 3:this.init3.apply(this,arguments);break;case 4:this.init4.apply(this,arguments);break}return}$$t.$h.init4.call(this,this.$tKey,this.$tValue,4,0,null)},init1:function($tKey,$tValue,a,b){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}$$t.$h.init4.call(this,this.$tKey,this.$tValue,4,b,null)},init2:function($tKey,$tValue,a,b){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}$$t.$h.init4.call(this,this.$tKey,this.$tValue,4,0,b)},init3:function($tKey,$tValue,a,b){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}$$t.$h.init1.call(this,this.$tKey,this.$tValue,1,b.count());var d=b.getEnumerator();while(d.moveNext()){var c=d.current();this.item(c.key(),c.value())}},init4:function($tKey,$tValue,a,b,c){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}$.ig.$op.init.call(this);this.a={};this.b={};this.j=c||$$t.$m.defaultEqualityComparerValue(this.$tKey);this.e=c==null&&($tKey===$.ig.String.prototype.$type||$tKey===String);this.d=$tKey===$.ig.Object.prototype.$type||$tKey.InstanceConstructor&&!$tKey.InstanceConstructor.prototype.getHashCode;this.c=c==null&&(this.e||this.d||$tKey.InstanceConstructor&&$tKey.InstanceConstructor.prototype.getHashCode==$.ig.Object.prototype.getHashCode)},count:function(){return this.k},item:function(key_,a){if(arguments.length===2){this.o(key_,a,false);return a}else{var result_=this.tryGetValueFast(key_);if(result_!==undefined){return result_}return $f(this.$tValue)}},length:function(){return this.k},containsKey:function(a){if(this.c){var b=this.n(a);return this.a.hasOwnProperty(b)}else{var c=this.l(a);var d=this.b[c];if(d){if(d.$isHashSetBucket){var g=d;for(var f=0;f<g.length;f++){var e=g[f];if(this.j.equalsC(e.key,a)){return true}}}else{return this.j.equalsC(d.key,a)}}}return false},remove:function(a){if(this.c){var b=this.n(a);if(!this.a.hasOwnProperty(b)){return false}delete this.a[b];delete this.b[b];this.k--;return true}var c=this.l(a);var d=this.b[c];if(d){if(d.$isHashSetBucket){var g=d;for(var f=0;f<g.length;f++){var e=g[f];if(this.j.equalsC(e.key,a)){d.removeItem(e);if(d.length==1){this.b[c]=d[0]}this.k--;return true}}}else{if(this.j.equalsC(d.key,a)){delete this.b[c];this.k--;return true}}}return false},clear:function(){this.k=0;this.a={};this.b={}},n:function(a){if(this.d){$.ig.util.ensureUniqueId(a)}if(this.e){return $n(this.$tKey,a)}else{return a.getHashCode().toString()}},l:function(a){if(this.d){$.ig.util.ensureUniqueId(a)}else{if(!a.getHashCode){this.d=true;$.ig.util.ensureUniqueId(a)}}return this.j.getHashCodeC(a)},add:function(a,b){this.o(a,b,true)},o:function(a,b,c){if(this.c){var d=this.n(a);if(!this.a.hasOwnProperty(d)){this.k++}else if(c){throw new $$6.d(1,"Duplicate key added to the dictionary")}this.a[d]=a;this.b[d]=b}else{var e=this.l(a);var f=this.b[e];if(f){if(f.$isHashSetBucket){var i=f;for(var h=0;h<i.length;h++){var g=i[h];if(this.j.equalsC(g.key,a)){if(c){throw new $$6.d(1,"Duplicate key added to the dictionary")}g.value=b;return}}f.push({key:a,value:b});this.k++}else{if(this.j.equalsC(f.key,a)){if(c){throw new $$6.d(1,"Duplicate key added to the dictionary")}f.value=b}else{var j=[f,{key:a,value:b}];j.$isHashSetBucket=true;this.b[e]=j;this.k++}}}else{this.b[e]={key:a,value:b};this.k++}}},tryGetValueFast:function(a){var b;if(this.c){var c=this.n(a);if(this.a.hasOwnProperty(c)){b=this.b[c];return b}}else{var d=this.l(a);var e=this.b[d];if(e){if(e.$isHashSetBucket){var h=e;for(var g=0;g<h.length;g++){var f=h[g];if(this.j.equalsC(f.key,a)){b=f.value;return b}}}else{if(this.j.equalsC(e.key,a)){b=e.value;return b}}}}return $v(this.$tValue,undefined)},tryGetValue:function(a,b){if(this.c){var c=this.n(a);if(this.a.hasOwnProperty(c)){b=this.b[c];return{ret:true,p1:b}}}else{var d=this.l(a);var e=this.b[d];if(e){if(e.$isHashSetBucket){var h=e;for(var g=0;g<h.length;g++){var f=h[g];if(this.j.equalsC(f.key,a)){b=f.value;return{ret:true,p1:b}}}}else{if(this.j.equalsC(e.key,a)){b=e.value;return{ret:true,p1:b}}}}}b=$f(this.$tValue);return{ret:false,p1:b}},isReadOnly:function(){return false},add1:function(a){this.add(a.key(),a.value())},contains:function(a){var $self=this;var b;return function(){var c=$self.tryGetValue(a.key(),b);b=c.p1;return c.ret}()&&$.ig.$op.equalsStatic($e(this.$tValue,b),$e(this.$tValue,a.value()))},copyTo:function(a,b){throw new $$6.q(0)},remove1:function(a){this.remove(a.key());return true},getEnumerator:function(){return this.g().getEnumerator()},g:function(){var d__=new $$t.j(this.$tKey,this.$tValue,(-2));d__.__4__this=this;return d__},h:function(){var d__=new $$t.k(this.$tKey,this.$tValue,(-2));d__.__4__this=this;return d__},i:function(){var d__=new $$t.l(this.$tKey,this.$tValue,(-2));d__.__4__this=this;return d__},keys:function(){return new $$t.i(this.$tKey,this.$tValue,this.$tKey,this,this.h(),this.j||$$t.$m.defaultEqualityComparerValue(this.$tKey))},values:function(){return new $$t.i(this.$tKey,this.$tValue,this.$tValue,this,this.i(),$$t.$m.defaultEqualityComparerValue(this.$tValue))},$type:new $.ig.Type("Dictionary$2",$.ig.$ot,[$$t.$t.$type.specialize(0,1),$$0.$bl.$type])},true);$c("Dictionary_EnumerableCollection$3:i","Object",{$tKey:null,$tValue:null,$t:null,c:null,b:null,a:null,init:function($tKey,$tValue,$t,a,b,c){this.$tKey=$tKey;this.$tValue=$tValue;this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue,this.$t)}$.ig.$op.init.call(this);this.c=b;this.b=c;this.a=a},count:function(){return this.a.count()},isReadOnly:function(){return true},add:function(a){throw new $$6.p(0)},clear:function(){throw new $$6.p(0)},contains:function(a){var c=this.c.getEnumerator();while(c.moveNext()){var b=c.current();if($.ig.$op.equalsStatic($e(this.$t,b),$e(this.$t,a))){return true}}return false},copyTo:function(a,b){var d=this.c.getEnumerator();while(d.moveNext()){var c=d.current();a[b++]=c}},remove:function(a){throw new $$6.p(0)},getEnumerator:function(){return this.c.getEnumerator()},getEnumerator1:function(){return this.c.getEnumerator()},$type:new $.ig.Type("Dictionary_EnumerableCollection$3",$.ig.$ot,[$$0.$bv.$type.specialize(2)])},true);$c("EqualityComparer$1:m","Object",{$t:null,init:function($t){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this)},defaultEqualityComparerValue:function($t){return new $$t.n($t)},equalsC:function(a,b){return this.equalsC($v(this.$t,a),$v(this.$t,b))},getHashCodeC:function(a){return this.getHashCodeC($v(this.$t,a))},$type:new $.ig.Type("EqualityComparer$1",$.ig.$ot,[$$0.$bj.$type,$$0.$by.$type.specialize(0)])},true);$c("DefaultEqualityComparer$1:n","EqualityComparer$1",{$t:null,init:function($t){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$$t.$m.init.call(this,this.$t)},equalsC:function(a,b){return $.ig.$op.equalsStatic($e(this.$t,a),$e(this.$t,b))},getHashCodeC:function(a){return a.getHashCode()},$type:new $.ig.Type("DefaultEqualityComparer$1",$$t.$m.$type.specialize(0))},true);$c("GenericEnumerable$1:o","Object",{$t:null,a:null,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this);this.a=a},getEnumerator:function(){return new $$t.p(this.$t,this.a().getEnumerator())},$type:new $.ig.Type("GenericEnumerable$1",$.ig.$ot,[$$0.$bw.$type.specialize(0)])},true);$c("GenericEnumerator$1:p","Object",{$t:null,a:null,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this);this.a=a},current:function(){return this.a.current()},moveNext:function(){return this.a.moveNext()},reset:function(){this.a.reset()},dispose:function(){},$type:new $.ig.Type("GenericEnumerator$1",$.ig.$ot,[$$0.$bx.$type.specialize(0)])},true);$c("HashSet$1:q","Object",{$t:null,k:null,n:0,__values:null,c:false,__stringPlaceholder:null,b:false,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break;case 2:this.init2.apply(this,arguments);break;case 3:this.init3.apply(this,arguments);break}return}$$t.$q.init2.call(this,this.$t,2,null)},init1:function($t,a,b){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$$t.$q.init3.call(this,this.$t,3,b,null)},init2:function($t,a,b){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this);this.__values={};this.k=b||$$t.$m.defaultEqualityComparerValue(this.$t);this.c=b==null&&($t===$.ig.String.prototype.$type||$t===String);this.b=$t===$.ig.Object.prototype.$type||$t.InstanceConstructor&&!$t.InstanceConstructor.prototype.getHashCode;this.__stringPlaceholder={}},init3:function($t,a,b,c){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$$t.$q.init2.call(this,this.$t,2,c);var e=b.getEnumerator();while(e.moveNext()){var d=e.current();this.add(d)}},isReadOnly:function(){return false},comparer:function(){return this.k},count:function(){return this.n},add:function(item_){if(this.c){if(this.__values[item_]){return false}this.__values[item_]=this.__stringPlaceholder;this.n++;return true}if(this.b){$.ig.util.ensureUniqueId(item_)}var a=this.k.getHashCodeC(item_);var current_=this.__values[a];if(current_){if(current_.$isHashSetBucket){var c=current_.getEnumerator();while(c.moveNext()){var b=c.current();if(this.k.equalsC(b,item_)){return false}}current_.push(item_)}else{if(this.k.equalsC(current_,item_)){return false}var bucket_=[current_,item_];bucket_.$isHashSetBucket=true;this.__values[a]=bucket_}}else{this.__values[a]=item_}this.n++;return true},clear:function(){this.n=0;this.__values={}},contains:function(item_){if(this.c){return!!this.__values[item_]}if(this.b){$.ig.util.ensureUniqueId(item_)}var a=this.k.getHashCodeC(item_);var current_=this.__values[a];if(current_!==undefined){if(current_.$isHashSetBucket){var c=current_.getEnumerator();while(c.moveNext()){var b=c.current();if(this.k.equalsC(b,item_)){return true}}}else{if(this.k.equalsC(current_,item_)){return true}}}return false},copyTo1:function(a){throw new $$6.q(0)},copyTo:function(a,b){throw new $$6.q(0)},copyTo2:function(a,b,c){throw new $$6.q(0)},createSetComparer:function($t){throw new $$6.q(0)},exceptWith:function(a){throw new $$6.q(0)},getEnumerator:function(){if(this.c){return $.ig.util.getArrayOfProperties(this.__values).getEnumerator()}var result_=new Array(0);var a=$.ig.util.getArrayOfValues(this.__values);for(var b=0;b<a.length;b++){var item_=a[b];if(item_.$isHashSetBucket){var c=item_.getEnumerator();while(c.moveNext()){var subItem_=c.current();result_.push(subItem_)}}else{result_.push(item_)}}return result_.getEnumerator()},intersectWith:function(a){throw new $$6.q(0)},isProperSubsetOf:function(a){throw new $$6.q(0)},isProperSupersetOf:function(a){throw new $$6.q(0)},isSubsetOf:function(a){throw new $$6.q(0)},isSupersetOf:function(a){throw new $$6.q(0)},onDeserialization:function(a){throw new $$6.q(0)},overlaps:function(a){if(this.count()>0){var c=a.getEnumerator();while(c.moveNext()){var b=c.current();if(this.contains(b)){return true}}}return false},remove:function(item_){if(this.c){if(this.__values[item_]){return false}delete this.__values[item_];this.n--;return true}if(this.b){$.ig.util.ensureUniqueId(item_)}var hashCode_=this.k.getHashCodeC(item_);var current_=this.__values[hashCode_];if(current_){if(current_.$isHashSetBucket){var a=current_.getEnumerator();while(a.moveNext()){var currentItem_=a.current();if(this.k.equalsC(currentItem_,item_)){current_.removeItem(currentItem_);if(current_.length==1){this.__values[hashCode_]=current_[0]}this.n--;return true}}}else{if(this.k.equalsC(current_,item_)){delete this.__values[hashCode_];this.n--;return true}}}return false},setEquals:function(a){throw new $$6.q(0)},symmetricExceptWith:function(a){throw new $$6.q(0)},trimExcess:function(){},unionWith:function(a){var c=a.getEnumerator();while(c.moveNext()){var b=c.current();this.add(b)}},$type:new $.ig.Type("HashSet$1",$.ig.$ot,[$$0.$bv.$type.specialize(0),$$0.$bw.$type.specialize(0),$$0.$bt.$type])},true);$c("IArrayList:r","Object",{$type:new $.ig.Type("IArrayList",null)},true);$c("IArray:s","Object",{$type:new $.ig.Type("IArray",null)},true);$c("LinkedList$1:v","Object",{$t:null,init:function($t){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this)},a:null,c:function(){return this.a},b:null,d:function(){return this.b},e:function(a){if(this.a==null){this.a=new $$t.w(this.$t,1,a);this.b=this.a}else{var b=this.a;this.a=new $$t.w(this.$t,1,a);this.a.a=b;b.b=this.a}},f:function(a){if(this.b==null){this.a=new $$t.w(this.$t,1,a);this.b=this.a}else{var b=this.b;this.b=new $$t.w(this.$t,1,a);this.b.b=b;b.a=this.b}},i:function(){this.h(this.c())},g:function(){this.a=null;this.b=null},h:function(a){if(this.a==a){this.a=a.a;if(a.a!=null){a.a.b=null}}if(this.b==a){this.b=a.b;if(a.b!=null){a.b.a=null}}if(a.b!=null){a.b.a=a.a}if(a.a!=null){a.a.b=a.b}a.a=null;a.b=null},$type:new $.ig.Type("LinkedList$1",$.ig.$ot)},true);$c("LinkedListNode$1:w","Object",{$t:null,_c:null,b:null,a:null,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}$.ig.$op.init.call(this)},init1:function($t,a,b){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this);this._c=b},$type:new $.ig.Type("LinkedListNode$1",$.ig.$ot)},true);$c("List$1:x","Object",{$t:null,__inner:null,c:false,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break;case 2:this.init2.apply(this,arguments);break}return}this.m={};$.ig.$op.init.call(this);this.__inner=[];this.c=this.$t.InstanceConstructor&&this.$t.InstanceConstructor.prototype.equals===$.ig.Object.prototype.equals},init1:function($t,a,b){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$$t.$x.init.call(this,this.$t,0);if(this.d(0,b,true)){return}var d=b.getEnumerator();while(d.moveNext()){var c=d.current();this.add(c)}},init2:function($t,a,b){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$$t.$x.init.call(this,this.$t,0)},x:function(a,b){this.__inner[a]=b},r:function(a,b){this.__inner.splice(a,0,b)},n:function(a){this.__inner.push(a)},u:function(a){if(a==0){this.__inner.shift();return}this.__inner.splice(a,1)},p:function(){this.__inner=[]},item:function(a,b){if(arguments.length===2){this.x(a,b);return b}else{return this.__inner[a]}},indexOf:function(a){if(this.c){return this.__inner.indexOf(a)}for(var b=0;b<this.__inner.length;b++){if($.ig.$op.equalsStatic($e(this.$t,a),$e(this.$t,this.__inner[b]))){return b}}return-1},indexOf2:function(a,b){if(this.c){return this.__inner.indexOf(a,b)}for(;b<this.__inner.length;b++){if($.ig.$op.equalsStatic($e(this.$t,a),$e(this.$t,this.__inner[b]))){return b}}return-1},lastIndexOf:function(a){if(this.c){return this.__inner.lastIndexOf(a)}for(var b=this.__inner.length-1;b>=0;b--){if($.ig.$op.equalsStatic($e(this.$t,a),$e(this.$t,this.__inner[b]))){return b}}return-1},insert:function(a,b){this.r(a,b)},removeAt:function(a){this.u(a)},count:function(){return this.__inner.length},isReadOnly:function(){return false},add:function(a){this.n(a)},clear:function(){this.p()},contains:function(a){return this.indexOf(a)>=0},copyTo:function(a,b){for(var c=0;c<this.__inner.length;c++){a[b+c]=this.__inner[c]}},remove:function(a){var b=this.indexOf(a);if(b<0){return false}this.u(b);return true},getEnumerator:function(){return this.__inner.getEnumerator()},asArray:function(){return this.__inner},d:function(index_,collection_,a){var b=$b($$t.$r.$type,collection_);if(b!=null){var a_=b.asArrayList();Array.prototype.splice.apply(this.__inner,Array.prototype.concat.apply([index_,0],a_));return true}var c=$b($$t.$s.$type,collection_);if(c!=null){var a_=c.asArray();Array.prototype.splice.apply(this.__inner,Array.prototype.concat.apply([index_,0],a_));return true}var asList_=$b($$0.$bz.$type.specialize(this.$t),collection_);if(asList_!=null){for(var i_=0;i_<asList_.count();i_++){var item_=asList_.item(i_);this.__inner.splice(index_+i_,0,item_)}return true}var arr_=Array.isArray(collection_)?collection_:null;if(arr_!=null){var inn_=this.__inner;if(this.__inner.length==0){if(a){var parr_=arr_;var d=arr_.length;for(var i_=0;i_<d;i_++){inn_[index_++]=parr_[i_]}}else{var e=arr_.length;for(var i_=0;i_<e;i_++){inn_[index_++]=$v(this.$t,arr_[i_])}}}else{for(var i_=0;i_<arr_.length;i_++){inn_.splice(index_++,0,arr_[i_])}}return true}return false},insertRange1:function(a,b){if(this.d(a,b,false)){return}var j_=a;var c=b.getEnumerator();while(c.moveNext()){var item_=c.current();this.__inner.splice(j_,0,item_);j_++}},insertRange:function(a,b){if(this.d(a,b,true)){return}var j_=a;var c=b.getEnumerator();while(c.moveNext()){var item_=c.current();this.__inner.splice(j_,0,item_);j_++}},removeRange:function(index_,numToRemove_){if(index_==0&&numToRemove_==1){this.__inner.shift();return}this.__inner.splice(index_,numToRemove_)},copyTo1:function(a,b){$.ig.util.arrayCopyTo(this.__inner,a,b)},isFixedSize:function(){return false},add1:function(a){this.n($v(this.$t,a));return this.__inner.length-1},contains1:function(a){return this.indexOf1(a)>=0},indexOf1:function(a){return this.indexOf($v(this.$t,a))},insert1:function(a,b){this.r(a,$v(this.$t,b))},remove1:function(a){var b=this.indexOf1(a);if(b<0){return}this.u(b)},sort:function(){var a=null;if(this.$t==Number){a=function(b,c){var d=b;var e=c;if(d<e){return-1}if(d==e){return 0}return 1}}else if(this.$t==$$0.$a8.$type){a=function(b,c){var d=b;var e=c;if(d<e){return-1}if(d==e){return 0}return 1}}else if(this.$t==$$0.$aw.$type){a=function(b,c){var d=$h(b);var e=$h(c);if(d<e){return-1}if(d==e){return 0}return 1}}else if(this.$t==$$0.$s.$type){a=function(b,c){var d=b;var e=c;if(d.getTime()<e.getTime()){return-1}if(d.getTime()==e.getTime()){return 0}return 1}}else{a=function(b,c){return b.compareTo(c)}}this.ab(a)},ab:function(compare_){this.__inner.sort(compare_)},sort2:function(compare_){this.__inner.sort(compare_)},_capacity:0,capacity:function(a){if(arguments.length===1){this._capacity=a;return a}else{return this._capacity}},addRange:function(a){var b=a.getEnumerator();while(b.moveNext()){var item_=b.current();this.__inner.push(item_)}},toArray:function(){return this.__inner},forEach:function(a){for(var b=0;b<this.__inner.length;b++){a(this.__inner[b])}},isSynchronized:function(){return true},m:null,syncRoot:function(){return this.m},binarySearch:function(a){return this.binarySearch1(a,$$6.$ab.defaultComparerValue(this.$t))},binarySearch1:function(a,b){var c=0;var d=this.count()-1;while(c<=d){var e=c+$a(d-c,2);var f=this.__inner[e];var g=b.compare(f,a);if(g==0){return e}if(g<0){c=e+1}else{d=e-1}}return~c},asReadOnly:function(){return new $$t.g(this.$t,1,this)},reverse:function(){for(var a=0;a<$a(this.count(),2);a++){var b=this.count()-a-1;var c=this.__inner[a];this.__inner[a]=this.__inner[b];this.__inner[b]=c}},sort1:function(a){this.sort2(a.compare.runOn(a))},findIndex:function(a){for(var b=0;b<this.__inner.length;b++){if(a(this.__inner[b])){return b}}return-1},removeAll:function(a){throw new $$6.q(0)},$type:new $.ig.Type("List$1",$.ig.$ot,[$$0.$bz.$type.specialize(0),$$t.$s.$type,$$0.$bm.$type])},true);$c("Queue$1:y","Object",{$t:null,d:0,e:0,f:0,a:null,init:function($t){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this);this.a=new Array(4)},count:function(){return this.d},copyTo:function(a,b){if(this.e<this.f){for(var c=this.e;c<this.f;c++){a[b++]=$e(this.$t,this.a[c])}}else{for(var d=this.e;d<this.a.length;d++){a[b++]=$e(this.$t,this.a[d])}for(var e=0;e<this.f;e++){a[b++]=$e(this.$t,this.a[e])}}},isSynchronized:function(){return false},syncRoot:function(){return null},getEnumerator:function(){var d__=new $$t.z(this.$t,0);d__.__4__this=this;return d__},enqueue:function(a){if(this.d==this.a.length){var b=new Array(Math.max(2,this.a.length*2));this.copyTo(b,0);this.e=0;this.f=this.a.length;this.a=b}this.a[this.f]=a;this.f=(this.f+1)%this.a.length;this.d++},dequeue:function(){if(this.d==0){throw new $$6.p(1,"The Queue is empty.")}var a=this.a[this.e];this.a[this.e]=null;this.e=(this.e+1)%this.a.length;this.d--;return a},peek:function(){if(this.d==0){throw new $$6.p(1,"The Queue is empty.")}return this.a[this.e]},$type:new $.ig.Type("Queue$1",$.ig.$ot,[$$0.$bk.$type,$$0.$bw.$type.specialize(0)])},true);$c("Stack$1:aa","Object",{$t:null,init:function($t){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}this.c=new $$0.bq;$.ig.$op.init.call(this)},c:null,push:function(a){this.c.add($e(this.$t,a))},peek:function(){if(this.c.length<1){return $f(this.$t)}return $v(this.$t,this.c[this.c.length-1])},pop:function(){var a=this.c[this.c.length-1];this.c.removeAt(this.c.length-1);return $v(this.$t,a)},count:function(){return this.c.length},clear:function(){this.c.clear()},contains:function(a){return this.c.contains($e(this.$t,a))},getEnumerator:function(){return new $$t.ab(this.$t,this.c)},toArray:function(){var a=this.c.length;var b=new Array(a);for(var c=0;c<a;c++){b[c]=$v(this.$t,this.c[a-(c+1)])}return b},$type:new $.ig.Type("Stack$1",$.ig.$ot,[$$0.$bw.$type.specialize(0)])},true);$c("ReverseArrayEnumerator$1:ab","Object",{$t:null,b:0,a:null,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this);this.a=a;this.b=a.length},current:function(){return $v(this.$t,this.a[this.b])},moveNext:function(){this.b--;return this.b>=0},reset:function(){this.b=this.a.length},dispose:function(){throw new $$6.q(0)},$type:new $.ig.Type("ReverseArrayEnumerator$1",$.ig.$ot,[$$0.$bx.$type.specialize(0)])},true);$c("ObservableCollection$1:f","List$1",{$t:null,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break;case 2:this.init2.apply(this,arguments);break}return}$$t.$x.init.call(this,this.$t,0)},init1:function($t,a,b){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$$t.$x.init1.call(this,this.$t,1,b)},init2:function($t,a,b){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$$t.$x.init2.call(this,this.$t,2,b)},x:function(a,b){var c=this.__inner[a];$$t.$x.x.call(this,a,b);if(this.propertyChanged!=null){this.ad(new $$0.b7("Item[]"))}if(this.collectionChanged!=null){var d=new $$t.ae(2,2,$e(this.$t,b),$e(this.$t,c),a);this.ac(d)}},p:function(){$$t.$x.p.call(this);if(this.propertyChanged!=null){this.ad(new $$0.b7("Count"));this.ad(new $$0.b7("Item[]"))}if(this.collectionChanged!=null){var a=new $$t.ae(0,4);this.ac(a)}},r:function(a,b){$$t.$x.r.call(this,a,b);if(this.propertyChanged!=null){this.ad(new $$0.b7("Count"));this.ad(new $$0.b7("Item[]"))}if(this.collectionChanged!=null){var c=new $$t.ae(1,0,$e(this.$t,b),a);this.ac(c)}},n:function(a){$$t.$x.n.call(this,a);if(this.propertyChanged!=null){this.ad(new $$0.b7("Count"));this.ad(new $$0.b7("Item[]"))}if(this.collectionChanged!=null){var b=new $$t.ae(1,0,$e(this.$t,a),this.count()-1);this.ac(b)}},u:function(a){var b=this.__inner[a];$$t.$x.u.call(this,a);if(this.propertyChanged!=null){this.ad(new $$0.b7("Count"));this.ad(new $$0.b7("Item[]"))}if(this.collectionChanged!=null){var c=new $$t.ae(1,1,$e(this.$t,b),a);this.ac(c)}},collectionChanged:null,propertyChanged:null,ad:function(a){if(this.propertyChanged!=null){this.propertyChanged(this,a)}},ac:function(a){if(this.collectionChanged!=null){this.collectionChanged(this,a)}},$type:new $.ig.Type("ObservableCollection$1",$$t.$x.$type.specialize(0),[$$t.$ac.$type,$$0.$b6.$type])},true);$c("ObjectCollection:d","ObservableCollection$1",{init:function(a){if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break;case 2:this.init2.apply(this,arguments);break}return}$$t.$f.init.call(this,$.ig.$op.$type,0)},init1:function(a,b){$$t.$f.init1.call(this,$.ig.$op.$type,1,b)},init2:function(a,b){$$t.$f.init2.call(this,$.ig.$op.$type,2,b)},$type:new $.ig.Type("ObjectCollection",$$t.$f.$type.specialize($.ig.$op.$type))},true);$c("Collection$1:e","List$1",{$t:null,init:function($t){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$$t.$x.init.call(this,this.$t,0)},ac:function(){return this},$type:new $.ig.Type("Collection$1",$$t.$x.$type.specialize(0))},true);$c("ReadOnlyCollection$1:g","Object",{$t:null,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}if(a>0){switch(a){case 1:this.init1.apply(this,arguments);break}return}this.c={};$.ig.$op.init.call(this)},init1:function($t,a,b){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}this.c={};$.ig.$op.init.call(this);this.a=b},a:null,item:function(a,b){if(arguments.length===2){this.a.item(a,b);return b}else{return this.a.item(a)}},indexOf:function(a){return this.a.indexOf(a)},insert:function(a,b){},removeAt:function(a){},count:function(){return this.a.count()},isReadOnly:function(){return true},add:function(a){},clear:function(){},contains:function(a){return this.a.contains(a)},copyTo:function(a,b){this.a.copyTo(a,b)},remove:function(a){return false},getEnumerator:function(){return this.a.getEnumerator()},isFixedSize:function(){return true},add1:function(a){return-1},contains1:function(a){return this.a.contains($v(this.$t,a))},indexOf1:function(a){return this.a.indexOf($v(this.$t,a));
},insert1:function(a,b){},remove1:function(a){},copyTo1:function(a,b){this.a.copyTo(a,b)},b:function(){return this.a},isSynchronized:function(){return true},c:null,syncRoot:function(){return this.c},$type:new $.ig.Type("ReadOnlyCollection$1",$.ig.$ot,[$$0.$bz.$type.specialize(0),$$0.$bm.$type])},true);$c("Dictionary___ToEnumerableValues__IteratorClass$2:l","Object",{$tKey:null,$tValue:null,g:0,e:null,a:null,h:0,b:null,i:0,f:null,c:null,j:0,__4__this:null,init:function($tKey,$tValue,a){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}$.ig.$op.init.call(this);this.g=a},moveNext:function(){while(this.g>=0){switch(this.g){case 0:this.g=-1;if(this.__4__this.c){this.a=$.ig.util.getArrayOfProperties(this.__4__this.a);this.h=0;this.g=1;break}this.b=$.ig.util.getArrayOfProperties(this.__4__this.b);this.i=0;this.g=3;break;case 1:this.g=-1;if(this.h<this.a.length){this.e=this.__4__this.b[this.a[this.h]];this.g=2;return true}break;case 2:this.g=-1;this.h++;this.g=1;break;case 3:this.g=-1;if(this.i<this.b.length){this.f=this.__4__this.b[this.b[this.i]];if(this.f.$isHashSetBucket){this.c=this.f;this.j=0;this.g=4;break}this.e=this.f.value;this.g=6;return true}break;case 4:this.g=-1;if(this.j<this.c.length){var subItem_=this.c[this.j];this.e=subItem_.value;this.g=5;return true}this.g=6;break;case 5:this.g=-1;this.j++;this.g=4;break;case 6:this.g=-1;this.i++;this.g=3;break}}return false},getEnumerator:function(){var d__;if(this.g==-2){this.g=0;d__=this}else{d__=new $$t.l(this.$tKey,this.$tValue,0);d__.__4__this=this.__4__this}return d__},reset:function(){throw new $$0.r(0)},dispose:function(){},current:function(){return this.e},current1:function(){return $e(this.$tValue,this.e)},$type:new $.ig.Type("Dictionary___ToEnumerableValues__IteratorClass$2",$.ig.$ot,[$$0.$bw.$type.specialize(1),$$0.$bt.$type,$$0.$bx.$type.specialize(1),$$0.$bu.$type,$$0.$au.$type])},true);$c("Queue___GetEnumerator__IteratorClass$1:z","Object",{$t:null,c:0,b:null,d:0,e:0,f:0,__4__this:null,init:function($t,a){this.$t=$t;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$t)}$.ig.$op.init.call(this);this.c=a},moveNext:function(){while(this.c>=0){switch(this.c){case 0:this.c=-1;if(this.__4__this.e<this.__4__this.f){this.d=this.__4__this.e;this.c=1;break}this.e=this.__4__this.e;this.c=3;break;case 1:this.c=-1;if(this.d<this.__4__this.f){this.b=this.__4__this.a[this.d];this.c=2;return true}break;case 2:this.c=-1;this.d++;this.c=1;break;case 3:this.c=-1;if(this.e<this.__4__this.a.length){this.b=this.__4__this.a[this.e];this.c=4;return true}this.f=0;this.c=5;break;case 4:this.c=-1;this.e++;this.c=3;break;case 5:this.c=-1;if(this.f<this.__4__this.f){this.b=this.__4__this.a[this.f];this.c=6;return true}break;case 6:this.c=-1;this.f++;this.c=5;break}}return false},reset:function(){throw new $$0.r(0)},dispose:function(){},current:function(){return this.b},current1:function(){return $e(this.$t,this.b)},$type:new $.ig.Type("Queue___GetEnumerator__IteratorClass$1",$.ig.$ot,[$$0.$bx.$type.specialize(0),$$0.$bu.$type,$$0.$au.$type])},true);$c("Dictionary___ToEnumerable__IteratorClass$2:j","Object",{$tKey:null,$tValue:null,g:0,f:null,a:null,h:0,b:null,i:0,e:null,c:null,j:0,__4__this:null,init:function($tKey,$tValue,a){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}this.f=new $$t.u(this.$tKey,this.$tValue);$.ig.$op.init.call(this);this.g=a},moveNext:function(){while(this.g>=0){switch(this.g){case 0:this.g=-1;if(this.__4__this.c){this.a=$.ig.util.getArrayOfProperties(this.__4__this.a);this.h=0;this.g=1;break}this.b=$.ig.util.getArrayOfProperties(this.__4__this.b);this.i=0;this.g=3;break;case 1:this.g=-1;if(this.h<this.a.length){this.f=new $$t.u(this.$tKey,this.$tValue,1,this.__4__this.a[this.a[this.h]],this.__4__this.b[this.a[this.h]]);this.g=2;return true}break;case 2:this.g=-1;this.h++;this.g=1;break;case 3:this.g=-1;if(this.i<this.b.length){this.e=this.__4__this.b[this.b[this.i]];if(this.e.$isHashSetBucket){this.c=this.e;this.j=0;this.g=4;break}this.f=new $$t.u(this.$tKey,this.$tValue,1,this.e.key,this.e.value);this.g=6;return true}break;case 4:this.g=-1;if(this.j<this.c.length){var subItem_=this.c[this.j];this.f=new $$t.u(this.$tKey,this.$tValue,1,subItem_.key,subItem_.value);this.g=5;return true}this.g=6;break;case 5:this.g=-1;this.j++;this.g=4;break;case 6:this.g=-1;this.i++;this.g=3;break}}return false},getEnumerator:function(){var d__;if(this.g==-2){this.g=0;d__=this}else{d__=new $$t.j(this.$tKey,this.$tValue,0);d__.__4__this=this.__4__this}return d__},reset:function(){throw new $$0.r(0)},dispose:function(){},current:function(){return this.f},current1:function(){return this.f},$type:new $.ig.Type("Dictionary___ToEnumerable__IteratorClass$2",$.ig.$ot,[$$0.$bw.$type.specialize($$t.$u.$type.specialize(0,1)),$$0.$bt.$type,$$0.$bx.$type.specialize($$t.$u.$type.specialize(0,1)),$$0.$bu.$type,$$0.$au.$type])},true);$c("Dictionary___ToEnumerableKeys__IteratorClass$2:k","Object",{$tKey:null,$tValue:null,g:0,e:null,a:null,h:0,b:null,i:0,f:null,c:null,j:0,__4__this:null,init:function($tKey,$tValue,a){this.$tKey=$tKey;this.$tValue=$tValue;if(!this.hasOwnProperty("$type")){this.$type=this.$type.specialize(this.$tKey,this.$tValue)}$.ig.$op.init.call(this);this.g=a},moveNext:function(){while(this.g>=0){switch(this.g){case 0:this.g=-1;if(this.__4__this.c){this.a=$.ig.util.getArrayOfProperties(this.__4__this.a);this.h=0;this.g=1;break}this.b=$.ig.util.getArrayOfProperties(this.__4__this.b);this.i=0;this.g=3;break;case 1:this.g=-1;if(this.h<this.a.length){this.e=this.__4__this.a[this.a[this.h]];this.g=2;return true}break;case 2:this.g=-1;this.h++;this.g=1;break;case 3:this.g=-1;if(this.i<this.b.length){this.f=this.__4__this.b[this.b[this.i]];if(this.f.$isHashSetBucket){this.c=this.f;this.j=0;this.g=4;break}this.e=this.f.key;this.g=6;return true}break;case 4:this.g=-1;if(this.j<this.c.length){var subItem_=this.c[this.j];this.e=subItem_.key;this.g=5;return true}this.g=6;break;case 5:this.g=-1;this.j++;this.g=4;break;case 6:this.g=-1;this.i++;this.g=3;break}}return false},getEnumerator:function(){var d__;if(this.g==-2){this.g=0;d__=this}else{d__=new $$t.k(this.$tKey,this.$tValue,0);d__.__4__this=this.__4__this}return d__},reset:function(){throw new $$0.r(0)},dispose:function(){},current:function(){return this.e},current1:function(){return $e(this.$tKey,this.e)},$type:new $.ig.Type("Dictionary___ToEnumerableKeys__IteratorClass$2",$.ig.$ot,[$$0.$bw.$type.specialize(0),$$0.$bt.$type,$$0.$bx.$type.specialize(0),$$0.$bu.$type,$$0.$au.$type])},true)});(function(factory){if(typeof define==="function"&&define.amd){define("watermark",["jquery"],factory)}else{factory(jQuery)}})(function($){$(document).ready(function(){var wm=$("#__ig_wm__").length>0?$("#__ig_wm__"):$("<div id='__ig_wm__'></div>").appendTo(document.body);wm.css({position:"fixed",bottom:0,right:0,zIndex:1e3}).addClass("ui-igtrialwatermark")})});