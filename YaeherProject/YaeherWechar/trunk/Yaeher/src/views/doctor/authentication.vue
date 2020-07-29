<template>
    <div class="authentication">
        <mt-header fixed title="怡禾认证">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content authBox">
            <img src="../../assets/image/authBg.jpg" alt="">
            <div class="authentication-btn">
                <router-link to="/agreement-login" class="okBtn">
                    <mt-button type="primary" class="mint-button--large">去认证</mt-button>
                </router-link>
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

.authentication {

    .authBox {
        margin-top: 40px;
        img {
            width: 100%;
            display: block;
        }
    }
    .authentication-btn {
        position: absolute;
        left: 10px;
        right: 10px;
        bottom: 10px;
    }
}

</style>