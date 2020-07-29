<template>
    <div class="patient-success">
        <!-- <mt-header fixed title="提交成功">
        </mt-header> -->
        <div class="content">
            <div class="userAvatar">
				<div class="userInfo">
					<p class="p1" >咨询提交成功</p>
                    <p v-if="serviceType === 'Phone'" class="p2"> 请注意接听 <span>&nbsp;0755-26788591&nbsp;</span> 的来电</p>
					<p class="p2">
                        <router-link to="/index-patient">返回首页</router-link>
                        <router-link to="/user-record">我的咨询</router-link>
                    </p>
				</div>
			</div>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            serviceType: ''
        }
    },
    created () {
        this.serviceType = this.$route.query.serviceType
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.patient-success {
    .userAvatar {width: 100%; height: 170px; display: flex; justify-content: center; align-items: center; background: $default-color;}
    .userInfo .p1 {color: $color-wfont; font-size: 30px; padding-bottom: 10px; text-align: center;}
    .userInfo .p2 {
        display: flex; 
        color: $color-wfont; 
        font-size: 14px;
        span {
            color: $color-afont;
            font-size: 16px;
        }

    }
    .userInfo .p2 a{color: $color-wfont; text-align: center; flex: 1; display: block;  text-align: center;}
}

</style>