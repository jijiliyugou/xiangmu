<template>
    <div class="refresh">
        <div class="noMore">
            <img src="../../assets/image/meiyou.png" alt="">
            <p>网络请求失败</p>
            <div class="aginRfs">
                <div @click="refreshAgin">重新加载</div>
            </div>    
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
        }
        
    },
    methods: {
        refreshAgin () {
            window.location.reload()
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.refresh {
    position: fixed;
    top: 42px;
    bottom: 0;
    left: 0;
    right: 0;
    overflow: hidden;
    z-index: 2000;
    background-color: #fff;
    .noMore {
		position: absolute;
		top: 100px;
		left: 50%;
		margin: 0px 0 0 -100px;
		width: 200px;
		height: 200px;
		p {
			position: absolute;
			left: 0;
			right: 0;
			bottom: 17px;
			text-align: center;
			font-size: 14px;
			color: #999;
		}
		img {
			display: block;
			width: 100%;
			height: 100%;
        }
        .aginRfs {
            text-align: center;
            div {
                width: 120px;
                display: inline-block;
                color: #666;
                font-size: 16px;
                border: 1px solid #999;
                padding: 6px 5px;
                border-radius: 3px;
            }
        }
	}
}

</style>