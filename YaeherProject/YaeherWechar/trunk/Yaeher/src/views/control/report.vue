<template>
    <div class="control-report padding-top">
        <mt-header fixed title="报表统计">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <ul class="user-list">
                <li>
                    <router-link to="/control-ranking" class="user-href">
                        <div class="flex">
                            <div class="user-title user-title1">医生排行</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
                <li>
                    <router-link to="/control-chargeback" class="user-href">
                        <div class="flex">
                            <div class="user-title user-title1">退单率统计</div>
                            <div class="href-arrow">
                                <i class="mint-cell-allow-right"></i>
                            </div>
                        </div>
                    </router-link>
                </li>
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