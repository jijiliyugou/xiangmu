<template>
    <div class="chat">
        <div class="content">
            <div class="weChat">
				<div class="weTime">
					<div>08月11日13:48</div>
				</div>
				<div class="weTitle infoTitle">
					<a href="doctorDetail.html"><img src="../../assets/image/lu.png" /></a>
					<p>
						您好！在吗？我有问题想请教您一下。
					</p>
				</div>
				<div class="weTime">
					<div>08月11日13:48</div>
				</div>
				<div class="weTitle">
					<a href="doctorDetail.html"><img src="../../assets/image/lu.png" /></a>
					<p>
						在的亲，请问有什么可以帮你的 ?
					</p>
				</div>
				<div class="weTime">
					<div>08月11日13:48</div>
				</div>
				<div class="weTitle infoTitle">
					<a href="doctorDetail.html"><img src="../../assets/image/lu.png" /></a>
					<p>
						您好！在吗？我有问题想请教您一下。
					</p>
				</div>
				<div class="weTime">
					<div>08月11日13:48</div>
				</div>
				<div class="weTitle">
					<a href="doctorDetail.html"><img src="../../assets/image/lu.png" /></a>
					<p>
						在的亲，请问有什么可以帮你的 ?
					</p>
				</div>
				<div class="weTime">
					<div>08月11日13:48</div>
				</div>
				<div class="weTitle infoTitle">
					<a href="doctorDetail.html"><img src="../../assets/image/lu.png" /></a>
					<p>
						您好！在吗？我有问题想请教您一下。
					</p>
				</div>
				<div class="weTime">
					<div>08月11日13:48</div>
				</div>
				<div class="weTitle">
					<a href="doctorDetail.html"><img src="../../assets/image/lu.png" /></a>
					<p>
						在的亲，请问有什么可以帮你的 ?
					</p>
				</div>
			</div>
			<div class="send-out">
				<span style="width: 10%;">语音</span>
				<span style="width: 80%; background: #fff;">输入文字</span>
				<span style="width: 10%;">+</span>
			</div>
        </div>
    </div>
</template>

<script>
export default {
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.chat {
	.weChat {padding: 10px 60px; background: #ddd; position: relative;}
	.weTime {display: flex; justify-content: center; align-items: center; padding-bottom: 10px;}
	.weTime div {padding: 3px 5px; background: #aaa; color: $color-wfont; font-size: $font-m; border-radius: 3px;}
	.weChat p {font-size: $font-m; color: #333;}
	.weTitle {background: $default-color; padding: 10px; position: relative; border-radius: 3px;}
	.weTitle img {position: absolute; right: -50px; top: 0; width: 40px; height: 40px; border-radius: 5px;}
	.infoTitle {background: $color-wfont;}
	.infoTitle p {color: #333;}
	.infoTitle img {left: -50px; top: 0; right: 0;}
	.send-out {height: 30px; border-top: 1px solid $color-cfont; text-align: center; padding: 10px 0; overflow: hidden; background: #ccc;}
	.send-out span {display: block; height: 100%; line-height: 29px; float: left;}
	.dapartOk button { border: none; color: $color-wfont; background: $default-color; width: 100%; height: 40px;}
	.weTime {margin-top: 10px;}
}


</style>