<template>
    <div class="patient-info padding-top">
        <mt-header fixed title="个人信息">
            <a @click="$router.push({path: '/user-patient'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <ul class="user-list">
                <li>
                    <router-link to="/patient-info-detail" class="user-href">
                        <div class="flex">
                            <div class="user-title user-title1">个人信息</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/user-number" class="user-href">
                        <div class="flex">
                            <div class="user-title user-title1">我的成员</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <!-- <li>
                    <router-link to="/coupon" class="user-href">
                        <div class="flex">
                            <div class="user-title user-title1">我的账户</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li> -->
            </ul>    
            
        </div>
    </div>
</template>

<script>
export default {
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
</style>