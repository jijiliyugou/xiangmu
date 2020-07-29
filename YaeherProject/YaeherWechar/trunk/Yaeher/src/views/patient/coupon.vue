<template>
    <div class="coupon padding-top">
        <mt-header fixed title="我的账户">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>        
        </mt-header>
        <div class="content">
            <ul class="user-list">
                <li>
                    <router-link to="/doctor-detail-patient" class="user-href">
                        <div class="flex">
                            <div class="user-title user-title1">账户余额：8000.00</div>
                            <div class="href-arrow">
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/doctor-detail-patient" class="user-href">
                        <div class="flex">
                            <div class="user-title user-title1">优惠券： 无</div>
                            <div class="href-arrow">
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>    
            
        </div>
    </div>
</template>

<style lang="scss">
@import "~assets/sass/base.scss";

</style>