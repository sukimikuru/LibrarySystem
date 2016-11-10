//原图/缩略图 的比例 >=1
var UserHeadUtil = {
	ratio: 1,
	view_H:200,
	view_W:200,
	initialize:function(path){
		$("#user_head_origin").attr("src", path);
		$("#user_head_upload_box").hide();
		$("#user_head_show_box").show();
		
		$("#headimg_120").attr("src", path);
		$("#headimg_94").attr("src", path);
		$("#headimg_48").attr("src", path);
		$("#headimg_28").attr("src", path);
		var img = new Image();
		img.src = path;
		if(img.width==0){
			var obj = this;
			img.onload = function(){ 
				obj.imgOperate(img);
			};
		}else{
			this.imgOperate(img);
		}
	},
	imgOperate:function(img){
		if(img){
			this.resize('user_head_origin', img.width, img.height, 200, 200);
			var x=0,y=0,size=0;
			if(this.view_W > this.view_H ){
				x = (this.view_W - this.view_H)/2;
				size = this.view_H;
			}else if(this.view_W < this.view_H){
				y = (this.view_H - this.view_W)/2;
				size = this.view_W;
			}else{
				size = this.view_W;
			}
			var obj = this;
			$('img#user_head_origin').imgAreaSelect({
		    	aspectRatio:"1:1",
		        handles: "corners",
		       	persistent:true,
		       	show:true,
				imageWidth: obj.view_W,
				imageHeight: obj.view_H,
				x1: x,
				y1: y,
				x2: x + size,
				y2: y + size,
				onSelectChange: function(img, selection){
				    obj.preview('headimg_120', obj.view_W, obj.view_H, selection.x1, selection.y1, selection.width, selection.height, 120, 120);
				    obj.preview('headimg_94', obj.view_W, obj.view_H, selection.x1, selection.y1, selection.width, selection.height, 94, 94);
				    obj.preview('headimg_48', obj.view_W, obj.view_H, selection.x1, selection.y1, selection.width, selection.height, 48, 48);
				    obj.preview('headimg_28', obj.view_W, obj.view_H, selection.x1, selection.y1, selection.width, selection.height, 28, 28);
					obj.setCutParams(selection.x1, selection.y1, selection.width, selection.height);
				}
			});
			this.preview('headimg_120', this.view_W, this.view_H, x, y, size, size, 120, 120);
			this.preview('headimg_94', this.view_W, this.view_H, x, y, size, size, 94, 94);
			this.preview('headimg_48', this.view_W, this.view_H, x, y, size, size, 48, 48);
			this.preview('headimg_28', this.view_W, this.view_H, x, y, size, size, 28, 28);
			this.setCutParams(x, y, size, size);
		}
	},
	resize:function(id, width, height, limit_W, limit_H){
		if(width>0 && height>0){
			if(width/height >= limit_W/limit_H){
				if(width > limit_W){
					this.view_W = limit_W;
					this.view_H = (limit_W/width)*height;
				}
			}else{
				if(height > limit_H){
					this.view_H = limit_H;
					this.view_W = (limit_H/height)*width;
				}
			}
			
			$('#'+id).attr( {
				"width" : this.view_W,
				"height" : this.view_H
			});
			
			this.ratio = width / this.view_W;
		}
	},

	preview:function(id, width, height, x, y, cut_W, cut_H, show_W, show_H){
		var scaleX = show_W / (cut_W * this.ratio || 1);
		var scaleY = show_H / (cut_H * this.ratio || 1);
		$('#'+id).css({
			width: Math.round(scaleX * width * this.ratio) + 'px',
			height: Math.round(scaleY * height * this.ratio) + 'px',
			marginLeft: '-' + Math.round(scaleX * x * this.ratio) + 'px',
			marginTop: '-' + Math.round(scaleY * y * this.ratio) + 'px'
		}); 
	},
	setCutParams:function(x, y, width, height){
		$('#head_x').val(Math.round(x * this.ratio));
		$('#head_y').val(Math.round(y * this.ratio));
		$('#head_width').val(Math.round(width * this.ratio));
		$('#head_height').val(Math.round(height * this.ratio));
	}
};

function cancelHead(){
	$('img#user_head_origin').imgAreaSelect({ remove: true });
	$("#user_head_show_box").hide();
	$("#user_head_upload_box").show();
	$("#headimg_120").attr("src", $('#img120x120').val()).css({
	    width: 120 + 'px',
	    height: 120 + 'px',
	    marginLeft: 0,
	    marginTop: 0
	});
	$("#headimg_94").attr("src", $('#img94x94').val()).css({
	    width: 94 + 'px',
	    height: 94 + 'px',
	    marginLeft: 0,
	    marginTop: 0
	});
	$("#headimg_48").attr("src", $('#img48x48').val()).css({
	    width: 48 + 'px',
	    height: 48 + 'px',
	    marginLeft: 0,
	    marginTop: 0
	});
	$("#headimg_28").attr("src", $('#img28x28').val()).css({
	    width: 28 + 'px',
	    height: 28 + 'px',
	    marginLeft: 0,
	    marginTop: 0
	});
	var file = $("#loadFile")
	file.after(file.clone().val(""));
	file.remove();
}

function closeImgSelection() {
    $('img#user_head_origin').imgAreaSelect({ remove: true });
    $("#user_head_show_box").hide();
    $("#user_head_upload_box").show();
}