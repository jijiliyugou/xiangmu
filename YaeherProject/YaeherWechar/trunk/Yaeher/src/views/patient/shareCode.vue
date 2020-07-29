<template>
    <div class="shareCode">
        <!-- <mt-header fixed title="关注怡禾">
        </mt-header> -->
        <div class="content shareShow">
            <div class="imgCase">
                <img src="../../assets/image/yihejiankang.jpg" />
                <p>长按关注怡禾健康</p>
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
    },
    created () {
        
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.shareCode {
    height: 100%;
    background-color: #fff;
    .shareShow {
        background-color: #fff;
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        .imgCase {
            padding: 0px 50px;
            img {
                width: 100%;
                display: block;
            }
            p {
                text-align: center;
                padding: 10px;
                color: #999;
            }
        }
    }
    
}

</style>